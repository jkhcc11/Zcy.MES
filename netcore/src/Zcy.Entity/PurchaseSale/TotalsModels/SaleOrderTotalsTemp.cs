using System;

namespace Zcy.Entity.PurchaseSale.TotalsModels
{
    /// <summary>
    /// 销售按天统计Temp
    /// </summary>
    public class SaleOrderTotalsTemp
    {
        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime TotalsDate { get; set; }

        /// <summary>
        /// 当天运费价格
        /// </summary>
        public decimal FreightPrice { get; set; }

        /// <summary>
        /// 当天销售总价
        /// </summary>
        public decimal SumSalePrice { get; set; }

        /// <summary>
        /// 当天订单价格
        /// </summary>
        public decimal OrderPrice { get; set; }
    }
}
