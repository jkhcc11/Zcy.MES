namespace Zcy.Dto.DataReport
{
    /// <summary>
    /// 分页查询产品销售报表
    /// </summary>
    public class QueryPageProductSaleReportDto
    {
        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        public int? PurchaseCount { get; set; }

        /// <summary>
        /// 采购均价
        /// </summary>
        public decimal? AvgPurchaseUnitPrice { get; set; }

        /// <summary>
        /// 采购总价
        /// </summary>
        public decimal? SumPurchasePrice { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>
        public int SaleCount { get; set; }

        /// <summary>
        /// 销售均价
        /// </summary>
        public decimal AvgSaleUnitPrice { get; set; }

        /// <summary>
        /// 销售总价
        /// </summary>
        public decimal SumSalePrice { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        /// <remarks>
        /// 采购-销售
        /// </remarks>
        public int? StockCount => PurchaseCount - SaleCount;

        /// <summary>
        /// 盈亏
        /// </summary>
        public decimal? ProfitPrice => SumSalePrice - SumPurchasePrice;
    }
}
