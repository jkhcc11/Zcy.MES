namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 分页查询退货订单
    /// </summary>
    public class QueryPageReturnOrderDto : BaseQueryPageOrderDto
    {
        /// <summary>
        /// 供应商|客户Id
        /// </summary>
        public long SupplierClientId { get; set; }

        /// <summary>
        /// 客户名
        /// </summary>
        public string? SupplierClientName { get; set; }

        /// <summary>
        /// 送货人
        /// </summary>
        /// <remarks>
        ///  应该是客户那边的人
        ///  ？？这个不确定是不是客户那边的用户
        /// </remarks>
        public string? ShipmentUser { get; set; }

        /// <summary>
        /// 订单产品数量
        /// </summary>
        public int OrderProductCount { get; set; }
    }
}
