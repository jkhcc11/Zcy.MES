using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Service;
using Zcy.Dto.User;

namespace Zcy.IService.User
{
    /// <summary>
    /// 用户 服务接口
    /// </summary>
    public interface IUserService : IZcyBaseService
    {
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageUserDto>>> QueryUserAsync(QueryPageUserInput input);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<UserLoginDto>> UserLoginAsync(UserLoginInput input);

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateUserAsync(CreateUserInput input);

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ModifyUserInfoAsync(ModifyUserInfoInput input);

        /// <summary>
        /// 用户密码修改
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ModifyUserPwdAsync(ModifyUserPwdInput input);

        /// <summary>
        /// 启用/禁用 用户登录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> EnableUserLoginAsync(long userId);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteUserAsync(long userId);

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <remarks>
        /// 重置用户密码为123456
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult> ResetUserPwdAsync(long userId);

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> SetUserRoleAsync(SetUserRoleInput input);

        /// <summary>
        /// 初始化用户
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> InitUserAsync();

        /// <summary>
        /// 获取当前公司所有有效员工列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<GetCurrentCompanyValidEmployeeDto>>> GetCurrentCompanyValidEmployeeAsync();

        /// <summary>
        /// 用户离职
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DepartUserAsync(long userId);
        ///// <summary>
        ///// 批量导入用户
        ///// </summary>
        ///// <returns></returns>
        //Task<KdyResult> BatchImportUserAsync(BatchImportUserInput input);
    }
}
