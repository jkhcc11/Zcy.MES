using Microsoft.Extensions.DependencyInjection;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Dto.SysBaseInfo;
using Zcy.Entity.SysBaseInfo;
using Zcy.IRepository.SysBaseInfo;
using Zcy.IService.SysBaseInfo;

namespace Zcy.Service.SysBaseInfo
{
    /// <summary>
    /// 菜单 服务实现
    /// </summary>
    public class SystemMenuService : ZcyBaseService, ISystemMenuService
    {
        private readonly ISystemMenuRepository _systemMenuRepository;

        public SystemMenuService(IServiceCollection serviceCollection,
            ISystemMenuRepository systemMenuRepository) : base(serviceCollection)
        {
            _systemMenuRepository = systemMenuRepository;
        }

        /// <summary>
        /// 查询菜单列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageMenuDto>>> QueryPageMenuAsync(QueryPageMenuInput input)
        {
            var query = await _systemMenuRepository.GetQueryableAsync();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                query = query.Where(a => a.MenuName.Contains(input.KeyWord) ||
                                         a.MenuUrl.Contains(input.KeyWord) ||
                                         a.RouteName.Contains(input.KeyWord));
            }

            var dbList = await _systemMenuRepository.ToListAsync(query
                .OrderByDescending(a => a.OrderBy)
                .ThenByDescending(a => a.CreatedTime));
            var tempDto = BaseMapper.Map<IReadOnlyList<SystemMenu>, List<QueryPageMenuDto>>(dbList);
            var resultList = QueryPageMenuDto.GenerateMenuTree(tempDto);
            if (resultList == null ||
                resultList.Any() == false)
            {
                return KdyResult.Success(new QueryPageDto<QueryPageMenuDto>()
                {
                    Total = dbList.Count,
                    Items = new List<QueryPageMenuDto>()
                });
            }

            var resultDto = KdyResult.Success(new QueryPageDto<QueryPageMenuDto>()
            {
                Total = resultList.Count,
                Items = resultList.Skip((input.Page - 1) * input.PageSize).Take(input.PageSize).ToList()
            });
            return resultDto;
        }

        /// <summary>
        /// 获取所有菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<GetAllMenuTreeDto>>> GetAllMenuTreeAsync()
        {
            var query = await _systemMenuRepository.GetQueryableAsync();
            var dbList = await _systemMenuRepository.ToListAsync(query);
            var tempDto = BaseMapper.Map<IReadOnlyList<SystemMenu>, List<GetAllMenuTreeDto>>(dbList);
            var resultList = GetAllMenuTreeDto.GenerateMenuTree(tempDto);

            var resultDto = KdyResult.Success(resultList ?? new List<GetAllMenuTreeDto>());
            return resultDto;
        }

        /// <summary>
        /// 创建和更新菜单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateMenuAsync(CreateAndUpdateMenuInput input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateMenuAsync(input);
            }

            return await CreateMenuAsync(input);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input)
        {
            //todo:只实现了单删除
            if (input.Ids.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "Id不能为空");
            }

            var firstId = input.Ids.First();
            var dbEntity = await _systemMenuRepository.FirstOrDefaultAsync(firstId);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "删除失败，菜单不存在");
            }

            if (await _systemMenuRepository.AnyAsync(a => a.ParentMenuId == dbEntity.Id))
            {
                return KdyResult.Error(KdyResultCode.Error, "删除失败，该菜单下存在子节点");
            }

            await _systemMenuRepository.DeleteAsync(dbEntity);
            return KdyResult.Success("删除成功");
        }

        /// <summary>
        /// 获取所有一级菜单
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetAllOneMenuAsync()
        {
            var query = await _systemMenuRepository.GetQueryableAsync();
            query = query.Where(a => a.ParentMenuId == 0);

            var dbList = await _systemMenuRepository.ToListAsync(query);
            var tempDto = BaseMapper.Map<IReadOnlyList<SystemMenu>, IList<GetRoleActivateMenuDto>>(dbList);
            return KdyResult.Success(tempDto);
        }

        #region 私有
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> CreateMenuAsync(CreateAndUpdateMenuInput input)
        {
            if (await _systemMenuRepository.RouteNameExistAsync(input.RouteName))
            {
                return KdyResult.Error(KdyResultCode.Error, "当前角色已存在此路由名");
            }

            var dbEntity = new SystemMenu(input.MenuUrl, input.MenuName, input.RouteName)
            {
                IconPrefix = input.IconPrefix,
                Icon = input.Icon,
                IsRootPath = input.IsRootPath ?? false,
                IsCache = input.IsCache ?? false,
                LocalFilePath = input.LocalFilePath,
                OrderBy = input.OrderBy
            };
            dbEntity.SetParentMenuId(input.ParentMenuId ?? 0);

            await _systemMenuRepository.CreateAsync(dbEntity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> UpdateMenuAsync(CreateAndUpdateMenuInput input)
        {
            var dbEntity = await _systemMenuRepository.FirstOrDefaultAsync(input.Id.Value);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数错误");
            }

            if (await _systemMenuRepository.RouteNameExistAsync(input.Id.Value, input.RouteName))
            {
                return KdyResult.Error(KdyResultCode.Error, "当前角色已存在此路由名");
            }

            dbEntity.IconPrefix = input.IconPrefix;
            dbEntity.Icon = input.Icon;
            dbEntity.IsRootPath = input.IsRootPath ?? false;
            dbEntity.IsCache = input.IsCache ?? false;
            dbEntity.LocalFilePath = input.LocalFilePath;
            dbEntity.OrderBy = input.OrderBy;

            dbEntity.SetParentMenuId(input.ParentMenuId ?? 0);
            dbEntity.SetMenuUrl(input.MenuUrl);
            dbEntity.SetMenuName(input.MenuName);
            dbEntity.SetRouteName(input.RouteName);
            await _systemMenuRepository.UpdateAsync(dbEntity);
            return KdyResult.Success();
        }
        #endregion
    }
}
