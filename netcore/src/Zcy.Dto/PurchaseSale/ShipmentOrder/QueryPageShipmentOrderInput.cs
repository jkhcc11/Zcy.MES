using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.Entity.PurchaseSale;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 分页查询出货订单
    /// </summary>
    public class QueryPageShipmentOrderInput : BaseQueryPageOrderInput
    {
        /// <summary>
        /// 业务Id
        /// </summary>
        /// <remarks>
        ///  供应商Id|客户Id 等
        /// </remarks>
        [ZcyQuery(nameof(ShipmentOrder.SupplierClientId), ZcyOperator.Equal)]
        public override long? BusinessId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(ShipmentOrder.SupplierClientName), ZcyOperator.Like)]
        [ZcyQuery(nameof(ShipmentOrder.OrderNo), ZcyOperator.Like)]
        [ZcyQuery(nameof(ShipmentOrder.OrderRemark), ZcyOperator.Like)]
        [ZcyQuery(nameof(ShipmentOrder.ManagerUser), ZcyOperator.Like)]
        [ZcyQuery(nameof(ShipmentOrder.OrderSummary), ZcyOperator.Like)]
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
