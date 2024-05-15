using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Dto.SysBaseInfo;
using Zcy.IService.SysBaseInfo;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    [Authorize(Roles = AuthorizationConst.NormalRoleName.SuperAdmin)]
    public class RoleMenuController : BaseManagerController
    {
        private readonly ISystemMenuService _systemMenuService;
        private readonly ISystemRoleService _systemRoleService;
        private readonly ISystemRoleMenuService _systemRoleMenuService;

        public RoleMenuController(ISystemMenuService systemMenuService,
            ISystemRoleService systemRoleService,
            ISystemRoleMenuService systemRoleMenuService)
        {
            _systemMenuService = systemMenuService;
            _systemRoleService = systemRoleService;
            _systemRoleMenuService = systemRoleMenuService;
        }

        #region 菜单
        /// <summary>
        /// 创建和更新菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost("menu/create-and-update")]
        public async Task<KdyResult> CreateAndUpdateMenuAsync(CreateAndUpdateMenuInput input)
        {
            var result = await _systemMenuService.CreateAndUpdateMenuAsync(input);
            return result;
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("menu/query")]
        public async Task<KdyResult<QueryPageDto<QueryPageMenuDto>>> QueryPageRoleMenuAsync([FromQuery] QueryPageMenuInput input)
        {
            var result = await _systemMenuService.QueryPageMenuAsync(input);
            return result;
        }

        /// <summary>
        /// 获取所有菜单树
        /// </summary>
        /// <returns></returns>
        [HttpGet("menu/get-all-menu-tree")]
        public async Task<KdyResult<List<GetAllMenuTreeDto>>> GetAllMenuTreeAsync()
        {
            var result = await _systemMenuService.GetAllMenuTreeAsync();
            return result;
        }

        /// <summary>
        /// 获取所有一级菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("menu/get-all-one-menu")]
        public async Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetAllOneMenuAsync()
        {
            var result = await _systemMenuService.GetAllOneMenuAsync();
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("menu/delete")]
        public async Task<KdyResult> DeleteAsync(BatchOperationsInput input)
        {
            var result = await _systemMenuService.BatchDeleteAsync(input);
            return result;
        }
        #endregion

        #region 角色权限
        /// <summary>
        /// 创建和更新角色菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost("role-auth/create-and-update")]
        public async Task<KdyResult> CreateAndUpdateRoleMenuAsync(CreateAndUpdateRoleMenuInput input)
        {
            var result = await _systemRoleMenuService.CreateAndUpdateRoleMenuAsync(input);
            return result;
        }

        /// <summary>
        /// 获取角色已激活菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("role-auth/{roleId}")]
        public async Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetRoleActivateMenuAsync(long roleId)
        {
            var result = await _systemRoleMenuService.GetRoleActivateMenuAsync(roleId);
            return result;
        }
        #endregion

        #region 角色
        /// <summary>
        /// 创建和更新菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost("role/create-and-update")]
        public async Task<KdyResult> CreateAndUpdateRoleAsync(CreateAndUpdateRoleInput input)
        {
            var result = await _systemRoleService.CreateAndUpdateRoleAsync(input);
            return result;
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("role/query")]
        public async Task<KdyResult<QueryPageDto<QueryPageRoleDto>>> QueryPageRoleAsync([FromQuery] QueryPageRoleInput input)
        {
            var result = await _systemRoleService.QueryPageRoleAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("role/delete/{roleId}")]
        public async Task<KdyResult> DeleteRoleAsync(long roleId)
        {
            var result = await _systemRoleService.DeleteRoleAsync(roleId);
            return result;
        }
        #endregion
    }
}
