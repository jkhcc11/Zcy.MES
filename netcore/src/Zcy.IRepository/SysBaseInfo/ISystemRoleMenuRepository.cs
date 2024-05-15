using System.Collections.Generic;
using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.SysBaseInfo;

namespace Zcy.IRepository.SysBaseInfo
{
    /// <summary>
    /// 角色菜单 仓储接口
    /// </summary>
    public interface ISystemRoleMenuRepository : IBaseRepository<SystemRoleMenu, long>
    {
        /// <summary>
        /// 获取角色激活菜单
        /// </summary>
        /// <returns></returns>
        Task<List<SystemMenu>> GetActivatedMenuByRoleIdAsync(long roleId);

        /// <summary>
        /// 获取角色激活菜单(相同的去重)
        /// </summary>
        /// <returns></returns>
        Task<List<SystemMenu>> GetActivatedMenuByRoleIdAsync(long[] roleIds);

        /// <summary>
        /// 获取所有角色菜单配置
        /// </summary>
        /// <returns></returns>
        Task<List<SystemRoleMenu>> GetAllMenuByRoleIdAsync(long roleId);
    }
}
