using System;
using System.Collections.Generic;
using System.Linq;
using Zcy.Entity.FinancialMemo;

namespace Zcy.Entity.PurchaseSale
{
    /// <summary>
    /// 销售订单
    /// </summary>
    public class SaleOrder : BaseOrder
    {
        public const string OrderNoPrefix = "XS";

        /// <summary>
        /// 销售订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="orderNo">订单号</param>
        /// <param name="saleOrderStatus">订单状态</param>
        /// <param name="managerUserId">经办人Id</param>
        /// <param name="managerUser">经办人昵称</param>
        /// <param name="orderDate">订单日期</param>
        public SaleOrder(long orderId, string orderNo, long managerUserId,
            string managerUser, SaleOrderStatusEnum saleOrderStatus, DateTime orderDate)
            : base(orderId, orderNo, managerUserId, managerUser, orderDate)
        {
            SaleOrderStatus = saleOrderStatus;
        }

        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountTypeEnum? AccountType { get; set; }

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
        /// 销售单状态
        /// </summary>
        public SaleOrderStatusEnum SaleOrderStatus { get; protected set; }

        /// <summary>
        /// 运费价格
        /// </summary>
        public decimal FreightPrice { get; protected set; }

        /// <summary>
        /// 销售总价
        /// </summary>
        /// <remarks>
        ///  订单详情总价（不含运费）
        /// </remarks>
        public decimal SumSalePrice { get; protected set; }

        /// <summary>
        /// 订单价格
        /// </summary>
        /// <remarks>
        ///  运费+销售总价
        /// </remarks>
        public decimal OrderPrice { get; protected set; }

        public virtual ICollection<SaleOrderDetail>? OrderDetails { get; set; }

        /// <summary>
        /// 设置已完成
        /// </summary>
        public void SetFinished()
        {
            SaleOrderStatus = SaleOrderStatusEnum.Finished;
        }

        /// <summary>
        /// 设置运费
        /// </summary>
        public void SetFreightPrice(decimal freightPrice)
        {
            FreightPrice = freightPrice;
        }

        /// <summary>
        /// 计算订单价格
        /// </summary>
        public void TotalsPrice()
        {
            if (OrderDetails == null ||
                OrderDetails.Any() == false)
            {
                throw new ArgumentException("SaleOrder OrderDetails is null");
            }

            SumSalePrice = OrderDetails.Sum(a => a.SumPrice);
            OrderPrice = FreightPrice + SumSalePrice;
        }
    }
}
