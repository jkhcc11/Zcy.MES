using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Service;
using Zcy.Dto.SysBaseInfo;
using Zcy.Dto.User;
using Zcy.IService.SysBaseInfo;
using Zcy.IService.User;

namespace Zcy.MES.Controllers.Login
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : BaseLoginController
    {
        private readonly ISystemRoleMenuService _systemRoleMenuService;
        private readonly ILoginUserInfo _loginUserInfo;
        private readonly IUserService _userService;
        public UserController(ILoginUserInfo loginUserInfo, 
            ISystemRoleMenuService systemRoleMenuService,
            IUserService userService)
        {
            _loginUserInfo = loginUserInfo;
            _systemRoleMenuService = systemRoleMenuService;
            _userService = userService;
        }

        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-permission")]
        public async Task<KdyResult<List<GetRoleActivateMenuTreeDto>?>> UserLoginAsync()
        {
            var menu = await _systemRoleMenuService.GetRoleActivateMenuTreeAsync(_loginUserInfo.Roles.ToArray());
            return menu;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("modify-pwd")]
        public async Task<KdyResult> ModifyUserPwdAsync(ModifyUserPwdInput input)
        {
            var result = await _userService.ModifyUserPwdAsync(input);
            return result;
        }
    }
}
