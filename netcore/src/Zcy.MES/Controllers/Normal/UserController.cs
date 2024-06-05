using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.Dto.User;
using Zcy.IService.User;

namespace Zcy.MES.Controllers.Normal
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : BaseNormalController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<KdyResult<UserLoginDto>> UserLoginAsync(UserLoginInput input)
        {
            var result = await _userService.UserLoginAsync(input);
            return result;
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
