using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.Dto.Company;
using Zcy.Dto.User;
using Zcy.IService.User;
using Microsoft.AspNetCore.Authorization;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : BaseManagerController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        public async Task<KdyResult> CreateUserAsync(CreateUserInput input)
        {
            var result = await _userService.CreateUserAsync(input);
            return result;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("modify")]
        public async Task<KdyResult> ModifyUserInfoAsync(ModifyUserInfoInput input)
        {
            var result = await _userService.ModifyUserInfoAsync(input);
            return result;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageUserDto>>> QueryUserAsync(
            [FromQuery] QueryPageUserInput input)
        {
            var result = await _userService.QueryUserAsync(input);
            return result;
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <returns></returns>
        [HttpPost("set-user-role")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.SuperAdmin)]
        public async Task<KdyResult> SetUserRoleAsync(SetUserRoleInput input)
        {
            var result = await _userService.SetUserRoleAsync(input);
            return result;
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <returns></returns>
        [HttpPost("reset-pwd/{userId}")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.BossAndRoot)]
        public async Task<KdyResult> ResetUserPwdAsync(long userId)
        {
            var result = await _userService.ResetUserPwdAsync(userId);
            return result;
        }

        /// <summary>
        /// 启用/禁用 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpDelete("enable-or-disable/{userId}")]
        public async Task<KdyResult> EnableUserLoginAsync(long userId)
        {
            var result = await _userService.EnableUserLoginAsync(userId);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{userId}")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.BossAndRoot)]
        public async Task<KdyResult> DeleteAsync(long userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return result;
        }

        /// <summary>
        /// 用户离职
        /// </summary>
        /// <returns></returns>
        [HttpPost("depart/{userId}")]
        public async Task<KdyResult> DepartUserAsync(long userId)
        {
            var result = await _userService.DepartUserAsync(userId);
            return result;
        }
    }
}
