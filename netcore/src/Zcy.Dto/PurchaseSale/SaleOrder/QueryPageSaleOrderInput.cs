using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.Entity.FinancialMemo;
using Zcy.Entity.PurchaseSale;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 分页查询销售订单
    /// </summary>
    public class QueryPageSaleOrderInput : BaseQueryPageOrderInput
    {
        /// <summary>
        /// 账号类型
        /// </summary>
        [ZcyQuery(nameof(SaleOrder.AccountType), ZcyOperator.Equal)]
        [EnumDataType(typeof(AccountTypeEnum))]
        public AccountTypeEnum? AccountType { get; set; }

        /// <summary>
        /// 业务Id
        /// </summary>
        /// <remarks>
        ///  供应商Id|客户Id 等
        /// </remarks>
        [ZcyQuery(nameof(SaleOrder.SupplierClientId), ZcyOperator.Equal)]
        public override long? BusinessId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(SaleOrder.SupplierClientName), ZcyOperator.Like)]
        [ZcyQuery(nameof(SaleOrder.OrderNo), ZcyOperator.Like)]
        [ZcyQuery(nameof(SaleOrder.OrderRemark), ZcyOperator.Like)]
        [ZcyQuery(nameof(SaleOrder.ManagerUser), ZcyOperator.Like)]
        public override string? KeyWord { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [ZcyQuery(nameof(SaleOrder.OrderDate), ZcyOperator.GtEqual)]
        [DataType(DataType.DateTime, ErrorMessage = "开始时间错误")]
        public override DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [ZcyQuery(nameof(SaleOrder.OrderDate), ZcyOperator.LessEqual)]
        [DataType(DataType.DateTime, ErrorMessage = "结束时间错误")]
        public override DateTime? EndTime { get; set; }
    }
}
