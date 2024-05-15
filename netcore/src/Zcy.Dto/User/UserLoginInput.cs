using System.ComponentModel.DataAnnotations;

namespace Zcy.Dto.User
{
    /// <summary>
    /// 用户登录 input
    /// </summary>
    public class UserLoginInput
    {
        public UserLoginInput(string userName, string userPwd)
        {
            UserName = userName;
            UserPwd = userPwd;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string UserPwd { get; set; }
    }
}
