namespace Zcy.Entity.PurchaseSale
{
    /// <summary>
    /// 销售订单状态
    /// </summary>
    public enum SaleOrderStatusEnum : byte
    {
        /// <summary>
        /// 待付款
        /// </summary>
        WaitPay = 5,

        /// <summary>
        /// 已完成
        /// </summary>
        Finished = 100
    }
}
