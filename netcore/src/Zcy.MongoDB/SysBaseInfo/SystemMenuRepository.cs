using Zcy.Entity.SysBaseInfo;
using Zcy.IRepository.SysBaseInfo;

namespace Zcy.MongoDB.SysBaseInfo
{
    /// <summary>
    /// 菜单 仓储实现
    /// </summary>
    public class SystemMenuRepository : BaseMongodbRepository<SystemMenu, long>, ISystemMenuRepository
    {
        public SystemMenuRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {
        }

        /// <summary>
        /// 路由名是否存在
        /// </summary>
        /// <param name="routeName">路由名</param>
        /// <returns></returns>
        public async Task<bool> RouteNameExistAsync(string routeName)
        {
            return await AnyAsync(a => a.RouteName == routeName);
        }

        /// <summary>
        /// 路由名是否存在
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <param name="routeName">路由名</param>
        /// <remarks>
        /// 同一个路由名在一个角色只会存在一次
        /// </remarks>
        /// <returns></returns>
        public async Task<bool> RouteNameExistAsync(long menuId, string routeName)
        {
            return await AnyAsync(a => a.RouteName == routeName &&
                                       a.Id != menuId);
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<SystemMenu>> GetAllMenuAsync()
        {
            return await GetAllListAsync();
        }
    }
}
