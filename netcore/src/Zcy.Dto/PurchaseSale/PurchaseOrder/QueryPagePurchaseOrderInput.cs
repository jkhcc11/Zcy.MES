using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.Entity.FinancialMemo;
using Zcy.Entity.PurchaseSale;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 分页查询采购订单
    /// </summary>
    public class QueryPagePurchaseOrderInput : BaseQueryPageOrderInput
    {
        /// <summary>
        /// 账号类型
        /// </summary>
        [ZcyQuery(nameof(PurchaseOrder.AccountType), ZcyOperator.Equal)]
        [EnumDataType(typeof(AccountTypeEnum))]
        public AccountTypeEnum? AccountType { get; set; }

        /// <summary>
        /// 业务Id
        /// </summary>
        /// <remarks>
        ///  供应商Id|客户Id 等
        /// </remarks>
        [ZcyQuery(nameof(PurchaseOrder.SupplierClientId), ZcyOperator.Equal)]
        public override long? BusinessId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(PurchaseOrder.SupplierClientName), ZcyOperator.Like)]
        [ZcyQuery(nameof(PurchaseOrder.OrderNo), ZcyOperator.Like)]
        [ZcyQuery(nameof(PurchaseOrder.OrderRemark), ZcyOperator.Like)]
        [ZcyQuery(nameof(PurchaseOrder.ManagerUser), ZcyOperator.Like)]
        public override string? KeyWord { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataType(DataType.DateTime, ErrorMessage = "开始时间错误")]
        public override DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataType(DataType.DateTime, ErrorMessage = "结束时间错误")]
        public override DateTime? EndTime { get; set; }
    }
}
