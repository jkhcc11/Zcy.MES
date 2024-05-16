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
        public async Task<KdyResult> UserLoginAsync(UserLoginInput input)
        {
            var result = await _userService.UserLoginAsync(input);
            return result;
        }
    }
}
