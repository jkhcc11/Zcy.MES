﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.SysBaseInfo;

namespace Zcy.IRepository.SysBaseInfo
{
    /// <summary>
    /// 菜单 仓储接口
    /// </summary>
    public interface ISystemMenuRepository : IBaseRepository<SystemMenu, long>
    {
        /// <summary>
        /// 路由名是否存在
        /// </summary>
        /// <param name="routeName">路由名</param>
        /// <returns></returns>
        Task<bool> RouteNameExistAsync(string routeName);

        /// <summary>
        /// 路由名是否存在
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <param name="routeName">路由名</param>
        /// <remarks>
        /// 同一个路由名在一个角色只会存在一次
        /// </remarks>
        /// <returns></returns>
        Task<bool> RouteNameExistAsync(long menuId, string routeName);

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<SystemMenu>> GetAllMenuAsync();
    }
}
