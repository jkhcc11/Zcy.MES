using Zcy.Entity.FinancialMemo;
using Zcy.Entity.PurchaseSale;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 分页查询采购订单
    /// </summary>
    public class QueryPageSaleOrderDto : BaseQueryPageOrderDto
    {
        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountTypeEnum? AccountType { get; set; }

        /// <summary>
        /// 供应商|客户Id
        /// </summary>
        public long SupplierClientId { get; set; }

        /// <summary>
        /// 客户名
        /// </summary>
        public string? SupplierClientName { get; set; }

        /// <summary>
        /// 销售单状态
        /// </summary>
        public SaleOrderStatusEnum SaleOrderStatus { get; set; }

        /// <summary>
        /// 运费价格
        /// </summary>
        public decimal FreightPrice { get; set; }

        /// <summary>
        /// 销售总价
        /// </summary>
        /// <remarks>
        ///  订单详情总价（不含运费）
        /// </remarks>
        public decimal SumSalePrice { get; set; }

        /// <summary>
        /// 订单价格
        /// </summary>
        /// <remarks>
        ///  运费+销售总价
        /// </remarks>
        public decimal OrderPrice { get; set; }
    }
}
