using System;

namespace Zcy.Entity.PurchaseSale
{
    /// <summary>
    /// 采购订单详情
    /// </summary>
    public class PurchaseOrderDetail : BaseOrderDetail
    {
        /// <summary>
        /// 基础订单详情
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="productId">产品Id</param>
        /// <param name="count">数量</param>
        /// <param name="unitPrice">单价</param>
        public PurchaseOrderDetail(long orderId, long productId,
            int count, decimal unitPrice) 
            : base(orderId, productId, count)
        {
            UnitPrice = unitPrice;
        }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; protected set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal SumPrice { get; protected set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 计算总价
        /// </summary>
        public void TotalSumPrice()
        {
            if (Count == default || UnitPrice == default)
            {
                throw new ArgumentException($"{nameof(PurchaseOrderDetail)} TotalSumPrice is error，count or unit price is null");
            }

            if (IsLoose)
            {
                SumPrice = Count * UnitPrice;
                return;
            }

            SumPrice = Count * UnitPrice * SpecCount;
        }
    }
}
