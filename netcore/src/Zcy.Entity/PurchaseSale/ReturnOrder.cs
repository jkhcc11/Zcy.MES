using System;
using System.Collections.Generic;

namespace Zcy.Entity.PurchaseSale
{
    /// <summary>
    /// 退货订单
    /// </summary>
    /// <remarks>
    ///  这里的经办人 是收货人（本公司人员）
    /// </remarks>
    public class ReturnOrder : BaseOrder
    {
        public const string OrderNoPrefix = "TH";

        /// <summary>
        /// 退货订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="orderNo">订单号</param>
        /// <param name="managerUserId">经办人Id</param>
        /// <param name="managerUser">经办人昵称</param>
        /// <param name="orderDate">订单日期</param>
        public ReturnOrder(long orderId, string orderNo, long managerUserId,
            string managerUser, DateTime orderDate) 
            : base(orderId, orderNo, managerUserId, managerUser, orderDate)
        {

        }

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
        /// 订单详情
        /// </summary>
        public virtual ICollection<ReturnOrderDetail>? OrderDetails { get; set; }
    }
}
