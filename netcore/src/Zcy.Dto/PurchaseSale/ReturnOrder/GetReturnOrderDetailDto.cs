namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 获取退货订单详情
    /// </summary>
    public class GetReturnOrderDetailDto : BaseGetOrderDetailDto
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
        /// 送货人
        /// </summary>
        /// <remarks>
        ///  应该是客户那边的人
        ///  ？？这个不确定是不是客户那边的用户
        /// </remarks>
        public string? ShipmentUser { get; set; }

        /// <summary>
        /// 退货订单商品列表
        /// </summary>
        public List<GetReturnOrderDetailItemDto>? OrderDetails { get; set; }
    }

    /// <summary>
    /// 获取退货订单详情item
    /// </summary>
    public class GetReturnOrderDetailItemDto : BaseGetOrderDetailItemDto
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
