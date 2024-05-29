namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 获取出货订单详情
    /// </summary>
    public class GetShipmentOrderDetailDto : BaseGetOrderDetailDto
    {
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
        /// 提货人
        /// </summary>
        /// <remarks>
        ///  应该是客户那边的人
        ///  ？？这个不确定是不是客户那边的用户
        /// </remarks>
        public string? PickUpUser { get; set; }

        /// <summary>
        /// 出货订单商品列表
        /// </summary>
        public List<GetShipmentOrderDetailItemDto>? OrderDetails { get; set; }
    }

    /// <summary>
    /// 获取出货订单详情item
    /// </summary>
    public class GetShipmentOrderDetailItemDto : BaseGetOrderDetailItemDto
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
