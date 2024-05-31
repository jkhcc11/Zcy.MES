using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Service;
using Zcy.IService.SysBaseInfo;

namespace Zcy.MES.Controllers.Login
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : BaseLoginController
    {
        private readonly ISystemRoleMenuService _systemRoleMenuService;
        private readonly ILoginUserInfo _loginUserInfo;

        public UserController(ILoginUserInfo loginUserInfo, ISystemRoleMenuService systemRoleMenuService)
        {
            _loginUserInfo = loginUserInfo;
            _systemRoleMenuService = systemRoleMenuService;
        }

        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-permission")]
        public async Task<KdyResult> UserLoginAsync()
        {
            var menu = await _systemRoleMenuService.GetRoleActivateMenuTreeAsync(_loginUserInfo.Roles.ToArray());
            return menu;
        }
    }
}
