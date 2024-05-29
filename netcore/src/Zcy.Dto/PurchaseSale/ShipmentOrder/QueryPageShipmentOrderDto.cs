namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 分页查询出货订单
    /// </summary>
    public class QueryPageShipmentOrderDto : BaseQueryPageOrderDto
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
        /// 提货人
        /// </summary>
        /// <remarks>
        ///  应该是客户那边的人
        ///  ？？这个不确定是不是客户那边的用户
        /// </remarks>
        public string? PickUpUser { get; set; }
    }
}
