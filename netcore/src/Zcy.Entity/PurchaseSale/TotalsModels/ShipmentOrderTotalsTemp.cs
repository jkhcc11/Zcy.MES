using System;

namespace Zcy.Entity.PurchaseSale.TotalsModels
{
    /// <summary>
    /// 出货按天统计Temp
    /// </summary>
    public class ShipmentOrderTotalsTemp
    {
        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime TotalsDate { get; set; }

        /// <summary>
        /// 当天出货量
        /// </summary>
        public int SumProductCount { get; set; }
    }
}
