using System.ComponentModel.DataAnnotations;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.User
{
    /// <summary>
    /// 修改用户信息
    /// </summary>
    public class ModifyUserInfoInput
    {
        public ModifyUserInfoInput(string userNick)
        {
            UserNick = userNick;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [Required(ErrorMessage = "用户昵称不能为空")]
        [StringLength(EntityConst.CommonName)]
        public string UserNick { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        [StringLength(EntityConst.CommonName)]
        public string? UserPhone { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [StringLength(EntityConst.CommonName)]
        public string? UserNo { get; set; }

        /// <summary>
        /// 是否启用登录
        /// </summary>
        /// <remarks>
        ///  普通员工也是用户（后面可登录系统 报工等）
        /// </remarks>
        public bool IsEnableLogin { get; set; }

        /// <summary>
        /// 结算基数
        /// </summary>
        /// <remarks>
        ///  员工的薪资结算基数
        /// </remarks>
        public decimal? BaseSettlement { get; set; }
    }
}
