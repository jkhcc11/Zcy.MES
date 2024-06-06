using Zcy.Entity.FinancialMemo;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 分页查询采购订单
    /// </summary>
    public class QueryPagePurchaseOrderDto : BaseQueryPageOrderDto
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
        public decimal OrderPrice { get; set; }

        /// <summary>
        /// 订单商品总价
        /// </summary>
        public decimal SumProductPrice { get; set; }
    }
}
