using Microsoft.Extensions.DependencyInjection;
using Zcy.BaseInterface.BaseModel;
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
        public SystemRoleMenuService(IServiceCollection serviceCollection,
            ISystemRoleMenuRepository systemRoleMenuRepository) : base(serviceCollection)
        {
            _systemRoleMenuRepository = systemRoleMenuRepository;
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
            //1、当前db中角色所有菜单列表
            var dbRoleMenu = await _systemRoleMenuRepository.GetAllMenuByRoleIdAsync(input.RoleId);
            if (dbRoleMenu.Any() == false &&
                input.MenuItems.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数错误，请至少选择一项");
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
        public async Task<KdyResult<List<GetRoleActivateMenuTreeDto>?>> GetRoleActivateMenuTreeAsync(long[] roleIds)
        {
            var dbList = await _systemRoleMenuRepository.GetActivatedMenuByRoleIdAsync(roleIds);
            var tempDto = BaseMapper.Map<List<SystemMenu>, List<GetRoleActivateMenuTreeDto>>(dbList);
            return KdyResult.Success(GetRoleActivateMenuTreeDto.GenerateMenuTreeAndParentHandler(tempDto));

        }
    }
}
