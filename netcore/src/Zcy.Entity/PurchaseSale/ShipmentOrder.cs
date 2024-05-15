﻿namespace Zcy.Entity.PurchaseSale
{
    /// <summary>
    /// 出货订单
    /// </summary>
    /// <remarks>
    ///  这里的经办人 是发货人（本公司人员）
    /// </remarks>
    public class ShipmentOrder : BaseOrder
    {
        public const string OrderNoPrefix = "CH";

        /// <summary>
        /// 出货订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="orderNo">订单号</param>
        /// <param name="managerUserId">经办人Id</param>
        /// <param name="managerUser">经办人昵称</param>
        public ShipmentOrder(long orderId, string orderNo, long managerUserId, string managerUser)
            : base(orderId, orderNo, managerUserId, managerUser)
        {

        }

        /// <summary>
        /// 供应商|客户Id
        /// </summary>
        public long SupplierClientId { get; set; }

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
