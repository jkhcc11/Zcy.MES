using System.Collections.Generic;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.SysBaseInfo;
using Zcy.Entity.SysBaseInfo;
using Zcy.IRepository.SysBaseInfo;
using Zcy.IService.SysBaseInfo;

namespace Zcy.Service.SysBaseInfo
{
    /// <summary>
    /// 角色菜单 服务实现
    /// </summary>
    public class SystemRoleMenuService : ZcyBaseService, ISystemRoleMenuService
    {
        private readonly ISystemRoleMenuRepository _systemRoleMenuRepository;
        private readonly ISystemMenuRepository _systemMenuRepository;
        private readonly ISystemRoleService _systemRoleService;

        public SystemRoleMenuService(ISystemRoleMenuRepository systemRoleMenuRepository,
            ISystemMenuRepository systemMenuRepository, ISystemRoleService systemRoleService)
        {
            _systemRoleMenuRepository = systemRoleMenuRepository;
            _systemMenuRepository = systemMenuRepository;
            _systemRoleService = systemRoleService;
        }

        /// <summary>
        /// 获取角色已激活菜单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetRoleActivateMenuAsync(long roleId)
        {
            var dbList = await _systemRoleMenuRepository.GetActivatedMenuByRoleIdAsync(roleId);
            var result = BaseMapper.Map<List<SystemMenu>, IList<GetRoleActivateMenuDto>>(dbList);
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 创建和更新角色菜单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateRoleMenuAsync(CreateAndUpdateRoleMenuInput input)
        {
            var allRoleCache = await _systemRoleService.GetAllRoleCacheAsync();
            if (allRoleCache == null)
            {
                throw new ZcyCustomException("GetRoleActivateMenuTreeAsync allRoleCache is null");
            }

            if (allRoleCache.Any(a => a.RoleId == input.RoleId) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "角色不存在，请留意");
            }


            //1、当前db中角色所有菜单列表
            var dbRoleMenu = await _systemRoleMenuRepository.GetAllMenuByRoleIdAsync(input.RoleId);
            if (dbRoleMenu.Any() == false &&
                input.MenuItems.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数错误，请至少选择一项");
            }

            var menuQuery = await _systemMenuRepository.GetQueryableAsync();
            menuQuery = menuQuery.Where(a => input.MenuItems.Select(b => b.MenuId).Contains(a.Id));
            var menuCount = await _systemMenuRepository.CountAsync(menuQuery);
            if (menuCount != input.MenuItems.Count)
            {
                return KdyResult.Error(KdyResultCode.ParError, "菜单参数错误");
            }

            if (dbRoleMenu.Any())
            {
                await _systemRoleMenuRepository.DeleteAsync(dbRoleMenu);
            }

            var createRoleMenu = input.MenuItems
                .Select(a => new SystemRoleMenu(a.MenuId, input.RoleId, true))
                .ToList();
            await _systemRoleMenuRepository.CreateAsync(createRoleMenu);
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取角色已激活菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<GetRoleActivateMenuTreeDto>?>> GetRoleActivateMenuTreeAsync(string[] roleNames)
        {
            var allRoleCache = await _systemRoleService.GetAllRoleCacheAsync();
            if (allRoleCache == null)
            {
                throw new ZcyCustomException("GetRoleActivateMenuTreeAsync allRoleCache is null");
            }

            var roleIds = allRoleCache
                .Where(a => roleNames.Contains(a.RoleName))
                .Select(a => a.RoleId)
                .ToArray();

            var dbList = await _systemRoleMenuRepository.GetActivatedMenuByRoleIdAsync(roleIds);
            var tempDto = new List<GetRoleActivateMenuTreeDto>();
            if (dbList.Any())
            {
                tempDto = BaseMapper.Map<List<SystemMenu>, List<GetRoleActivateMenuTreeDto>>(dbList);
            }
            else if (LoginUserInfo.IsSuperAdmin)
            {
                return KdyResult.Success(BuildFirstAdmin());
            }

            return KdyResult.Success(GetRoleActivateMenuTreeDto.GenerateMenuTreeAndParentHandler(tempDto));

        }

        /// <summary>
        /// 超管第一次访问
        /// </summary>
        /// <returns></returns>
        private List<GetRoleActivateMenuTreeDto> BuildFirstAdmin()
        {
            var mainId = 1;
            var result = new List<GetRoleActivateMenuTreeDto>()
            {
                new("/system","Dashboard","dashboard")
                {
                    Id = mainId,
                    Icon = "icon-dashboard",
                    Children=new List<GetRoleActivateMenuTreeDto>()
                    {
                        new( "/system/menu-list","权限菜单","MenuIndex")
                        {
                            Id = 2,
                            ParentMenuId = mainId,
                            ParentPath = "/system",
                            LocalFilePath = "/system/role-menu/menu-index",
                            Cacheable = true,
                            IsRootPath = true
                        },
                        new("/system/role-list","角色列表","RoleIndex")
                        {
                            Id = 3,
                            ParentMenuId = mainId,
                            ParentPath = "/system",
                            LocalFilePath = "/system/role-menu/role-list",
                            Cacheable = true
                        },
                    }
                },
            };

            return result;
        }
    }
}
