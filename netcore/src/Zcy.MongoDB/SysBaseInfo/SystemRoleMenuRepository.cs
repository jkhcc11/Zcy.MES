using MongoDB.Driver;
using Zcy.Entity.SysBaseInfo;
using Zcy.IRepository.SysBaseInfo;

namespace Zcy.MongoDB.SysBaseInfo
{
    /// <summary>
    /// 角色菜单 仓储实现
    /// </summary>
    public class SystemRoleMenuRepository : BaseMongodbRepository<SystemRoleMenu, long>, ISystemRoleMenuRepository
    {
        public SystemRoleMenuRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {
        }

        /// <summary>
        /// 获取角色激活菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemMenu>> GetActivatedMenuByRoleIdAsync(long roleId)
        {
            var dbRoleMenus = await DbCollection
                .Find(a => a.RoleId == roleId &&
                           a.IsActivate)
                .ToListAsync();
            var menuIds = dbRoleMenus.Select(a => a.MenuId).ToArray();
            if (menuIds.Any() == false)
            {
                return new List<SystemMenu>();
            }

            var menuDbName = $"{MongoDBConsts.DbPrefix}{nameof(SystemMenu)}";
            var menuCollection = ZcyMongodbContext.Database.GetCollection<SystemMenu>(menuDbName);
            var dbMenus = await menuCollection.Find(a => menuIds.Contains(a.Id)).ToListAsync();
            return dbMenus;
        }

        /// <summary>
        /// 获取角色激活菜单(相同的去重)
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemMenu>> GetActivatedMenuByRoleIdAsync(long[] roleIds)
        {
            var dbRoleMenus = await DbCollection
                .Find(a => roleIds.Contains(a.RoleId) &&
                           a.IsActivate)
                .ToListAsync();
            var menuIds = dbRoleMenus.Select(a => a.MenuId).ToArray();
            if (menuIds.Any() == false)
            {
                return new List<SystemMenu>();
            }

            var menuDbName = $"{MongoDBConsts.DbPrefix}{nameof(SystemMenu)}";
            var menuCollection = ZcyMongodbContext.Database.GetCollection<SystemMenu>(menuDbName);
            var dbMenus = await menuCollection
                .Find(a => menuIds.Contains(a.Id))
                .ToListAsync();
            return dbMenus.Distinct().ToList();
        }

        /// <summary>
        /// 获取所有菜单配置
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemRoleMenu>> GetAllMenuByRoleIdAsync(long roleId)
        {
            var dbRoleMenus = await DbCollection
                .Find(a => a.RoleId == roleId)
                .ToListAsync();
            return dbRoleMenus;
        }
    }
}
