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

        /// <summary>
        /// 小程序登录信息
        /// </summary>
        /// <remarks>
        ///  todo:临时配置 后面后台动态配置
        /// </remarks>
        /// <returns></returns>
        [HttpGet("get-login-info-wx")]
        public async Task<KdyResult<GetLoginInfoWithWxDto>> GetLoginInfoWithWxAsync()
        {
            var resultDto = new GetLoginInfoWithWxDto
            {
                IsBan = (await _userService.IsNormalAsync(_loginUserInfo.GetUserId())) == false
            };

            if (resultDto.IsBan)
            {
                return KdyResult.Success(resultDto);
            }

            var isNormal = _loginUserInfo.IsNormal;
            if (isNormal)
            {
                resultDto.MeReport = true;
                return KdyResult.Success(resultDto);
            }

            if (_loginUserInfo.IsNotBossAndRoot)
            {
                //管理
                resultDto.SetAdmin();
                return KdyResult.Success(resultDto);
            }

            //超管或boss
            resultDto.SetBoss();
            return KdyResult.Success(resultDto);
        }
    }
}
