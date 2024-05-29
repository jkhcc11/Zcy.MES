using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.Entity.PurchaseSale;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 分页查询退货订单
    /// </summary>
    public class QueryPageReturnOrderInput : BaseQueryPageOrderInput
    {
        /// <summary>
        /// 业务Id
        /// </summary>
        /// <remarks>
        ///  供应商Id|客户Id 等
        /// </remarks>
        [ZcyQuery(nameof(ReturnOrder.SupplierClientId), ZcyOperator.Equal)]
        public override long? BusinessId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(ReturnOrder.SupplierClientName), ZcyOperator.Like)]
        [ZcyQuery(nameof(ReturnOrder.OrderNo), ZcyOperator.Like)]
        [ZcyQuery(nameof(ReturnOrder.OrderRemark), ZcyOperator.Like)]
        [ZcyQuery(nameof(ReturnOrder.ManagerUser), ZcyOperator.Like)]
        [ZcyQuery(nameof(ReturnOrder.ShipmentUser), ZcyOperator.Like)]
        public override string? KeyWord { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [ZcyQuery(nameof(ReturnOrder.OrderDate), ZcyOperator.GtEqual)]
        [DataType(DataType.DateTime, ErrorMessage = "开始时间错误")]
        public override DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [ZcyQuery(nameof(ReturnOrder.OrderDate), ZcyOperator.LessEqual)]
        [DataType(DataType.DateTime, ErrorMessage = "结束时间错误")]
        public override DateTime? EndTime { get; set; }
    }
}
