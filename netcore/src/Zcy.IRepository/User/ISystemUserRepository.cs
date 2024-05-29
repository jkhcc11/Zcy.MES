using System.Collections.Generic;
using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.SysBaseInfo;
using Zcy.Entity.User;

namespace Zcy.IRepository.User
{
    /// <summary>
    /// 用户仓储 接口
    /// </summary>
    public interface ISystemUserRepository : IBaseRepository<SystemUser, long>
    {
        /// <summary>
        /// 根据用户名查用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        Task<SystemUser?> FindUserByUserNameAsync(string userName);

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <returns></returns>
        Task<List<SystemRole>> GetUserRoleAsync(long userId);

        /// <summary>
        /// 获取公司员工列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="loginUserId">登录用户ID</param>
        /// <returns></returns>
        Task<List<SystemUser>> GetCompanyAllEmployeeAsync(long companyId, long loginUserId);
    }
}
