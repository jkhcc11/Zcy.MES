using System;

namespace Zcy.Entity.PurchaseSale
{
    /// <summary>
    /// 退货订单详情
    /// </summary>
    public class ReturnOrderDetail : BaseOrderDetail
    {
        /// <summary>
        /// 退货订单详情
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="productId">产品Id</param>
        /// <param name="count">数量</param>
        public ReturnOrderDetail(long orderId, long productId, int count)
            : base(orderId, productId, count)
        {
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
