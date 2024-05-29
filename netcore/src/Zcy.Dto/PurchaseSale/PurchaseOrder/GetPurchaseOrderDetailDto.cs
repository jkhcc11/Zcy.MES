using Zcy.Entity.FinancialMemo;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 获取采购订单详情
    /// </summary>
    public class GetPurchaseOrderDetailDto : BaseGetOrderDetailDto
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
        /// 供应商|客户 名
        /// </summary>
        /// <remarks>
        /// 创建订单时的名称，修改后根据供应商|客户Id来
        /// </remarks>
        public string SupplierClientName { get; set; }

        /// <summary>
        /// 订单价格
        /// </summary>
        /// <remarks>
        /// 可能包含商品价格以外的价格
        /// </remarks>
        public decimal OrderPrice { get; set; }

        /// <summary>
        /// 订单商品总价
        /// </summary>
        public decimal SumProductPrice { get; set; }

        /// <summary>
        /// 采购订单商品列表
        /// </summary>
        public List<GetPurchaseOrderDetailItemDto>? OrderDetails { get; set; }
    }

    /// <summary>
    /// 获取采购订单详情item
    /// </summary>
    public class GetPurchaseOrderDetailItemDto : BaseGetOrderDetailItemDto
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
