using MongoDB.Driver;
using Zcy.Entity.SysBaseInfo;
using Zcy.Entity.User;
using Zcy.IRepository.User;

namespace Zcy.MongoDB.User
{
    /// <summary>
    /// 用户仓储 实现
    /// </summary>
    public class SystemUserRepository : BaseMongodbRepository<SystemUser, long>, ISystemUserRepository
    {
        public SystemUserRepository(ZcyMongodbContext zcyDbContext)
            : base(zcyDbContext)
        {
        }

        /// <summary>
        /// 根据用户名查用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<SystemUser?> FindUserByUserNameAsync(string userName)
        {
            var query = await GetQueryableAsync();
            var entity = await ToMongoQueryable(query.Where(a => a.UserName == userName))
                .FirstOrDefaultAsync();
            return entity;
        }

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemRole>> GetUserRoleAsync(long userId)
        {
            var userUserRoleDbName = $"{MongoDBConsts.DbPrefix}{nameof(SystemUserRole)}";
            var userUserRoleCollection = ZcyMongodbContext.Database.GetCollection<SystemUserRole>(userUserRoleDbName);

            var roleDbName = $"{MongoDBConsts.DbPrefix}{nameof(SystemRole)}";
            var roleCollection = ZcyMongodbContext.Database.GetCollection<SystemRole>(roleDbName);

            var userRole = await userUserRoleCollection
                .Find(a => a.UserId == userId &&
                           a.IsDelete == false)
                .ToListAsync();
            var userRoleIds = userRole.Select(a => a.RoleId).ToList();

            var roleList = await roleCollection
                .Find(a => userRoleIds.Contains(a.Id) &&
                           a.IsDelete == false)
                .ToListAsync();
            return roleList;
        }

        /// <summary>
        /// 获取公司员工列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="loginUserId">登录用户ID</param>
        /// <returns></returns>
        public async Task<List<SystemUser>> GetCompanyAllEmployeeAsync(long companyId, long loginUserId)
        {
            var query = await GetQueryableAsync();
            var dbList = await ToMongoQueryable(query.Where(a => a.CompanyId == companyId &&
                                                                 a.Id != loginUserId))
                .ToListAsync();
            return dbList;
        }
    }
}
