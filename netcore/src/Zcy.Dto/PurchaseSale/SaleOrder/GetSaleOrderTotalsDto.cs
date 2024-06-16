namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 获取销售订单汇总
    /// </summary>
    public class GetSaleOrderTotalsDto
    {
        /// <summary>
        /// 运费总价格
        /// </summary>
        public decimal SumFreightPrice { get; set; }

        /// <summary>
        /// 销售总价
        /// </summary>
        /// <remarks>
        ///  订单详情总价（不含运费）
        /// </remarks>
        public decimal SumSalePrice { get; set; }

        /// <summary>
        /// 订单总价格
        /// </summary>
        /// <remarks>
        ///  运费+销售总价
        /// </remarks>
        public decimal SumOrderPrice { get; set; }
    }
}
