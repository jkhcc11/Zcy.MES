using System;

namespace Zcy.Entity.PurchaseSale.TotalsModels
{
    /// <summary>
    /// 采购按天统计Temp
    /// </summary>
    public class PurchaseOrderTotalsTemp
    {
        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime TotalsDate { get; set; }

        /// <summary>
        /// 当天采购总价
        /// </summary>
        public decimal SumProductPrice { get; set; }

        /// <summary>
        /// 当天订单价格
        /// </summary>
        public decimal OrderPrice { get; set; }
    }
}
