using Zcy.Entity.FinancialMemo;
using Zcy.Entity.PurchaseSale;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 获取销售订单详情
    /// </summary>
    public class GetSaleOrderDetailDto : BaseGetOrderDetailDto
    {
        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountTypeEnum? AccountType { get; set; }

        /// <summary>
        /// 供应商|客户Id
        /// </summary>
        /// <remarks>
        /// todo:对于散户订单 不友好，根据情况设置是否可选
        /// </remarks>
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

        /// <summary>
        /// 销售订单商品列表
        /// </summary>
        public List<GetSaleOrderDetailItemDto>? OrderDetails { get; set; }
    }

    /// <summary>
    /// 获取销售订单详情item
    /// </summary>
    public class GetSaleOrderDetailItemDto : BaseGetOrderDetailItemDto
    {
        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal SumPrice { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
