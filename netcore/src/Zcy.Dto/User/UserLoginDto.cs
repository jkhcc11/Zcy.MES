namespace Zcy.Dto.User
{
    /// <summary>
    /// 用户登录 dto
    /// </summary>
    public class UserLoginDto
    {
        public UserLoginDto(long companyId, string userNick, string accessToken)
        {
            CompanyId = companyId;
            UserNick = userNick;
            AccessToken = accessToken;
        }

        /// <summary>
        /// 公司ID
        /// </summary>
        public long CompanyId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string? UserNo { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserNick { get; set; }

        /// <summary>
        /// 结算基数
        /// </summary>
        /// <remarks>
        ///  员工的薪资结算基数
        /// </remarks>
        public decimal BaseSettlement { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
