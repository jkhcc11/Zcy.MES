namespace Zcy.Entity.FinancialMemo
{
    /// <summary>
    /// 账户类型
    /// </summary>
    public enum AccountTypeEnum : byte
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        AliPay = 1,

        /// <summary>
        /// 微信
        /// </summary>
        Wx,

        /// <summary>
        /// 扫码支付
        /// </summary>
        ScanQr,

        /// <summary>
        /// 银行卡
        /// </summary>
        Bank,

        /// <summary>
        /// 其他
        /// </summary>
        Other = 100
    }
}
