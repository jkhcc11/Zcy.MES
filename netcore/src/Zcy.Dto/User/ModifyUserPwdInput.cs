using System.ComponentModel.DataAnnotations;

namespace Zcy.Dto.User
{
    /// <summary>
    /// 用户密码修改
    /// </summary>
    public class ModifyUserPwdInput
    {
        public ModifyUserPwdInput(string oldPwd, string newPwd, string newPwd1)
        {
            OldPwd = oldPwd;
            NewPwd = newPwd;
            NewPwd1 = newPwd1;
        }

        /// <summary>
        /// 旧密码
        /// </summary>
        [Required(ErrorMessage = "旧密码不能为空")]
        [StringLength(20, MinimumLength = 6,ErrorMessage = "密码长度为6-20位")]
        public string OldPwd { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度为6-20位")]
        public string NewPwd { get; set; }

        /// <summary>
        /// 新密码1
        /// </summary>
        [Required(ErrorMessage = "新密码1不能为空")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度为6-20位")]
        public string NewPwd1 { get; set; }
    }
}
