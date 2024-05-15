namespace Zcy.Dto.User
{
    /// <summary>
    /// 创建用户
    /// </summary>
    public class CreateUserInput
    {
        public CreateUserInput(string userName, string userNick)
        {
            UserName = userName;
            UserNick = userNick;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserNick { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string? UserNo { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        /// <remarks>
        /// 超管Id为0
        /// </remarks>
        public long CompanyId { get; set; }

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
        public decimal BaseSettlement { get; set; }
    }
}
