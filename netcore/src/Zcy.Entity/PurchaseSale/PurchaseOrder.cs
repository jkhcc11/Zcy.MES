using System;
using System.Collections.Generic;
using System.Linq;
using Zcy.Entity.FinancialMemo;

namespace Zcy.Entity.PurchaseSale
{
    /// <summary>
    /// 采购订单
    /// </summary>
    public class PurchaseOrder : BaseOrder
    {
        public const string OrderNoPrefix = "CG";

        /// <summary>
        /// 采购订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="orderNo">订单号</param>
        /// <param name="managerUserId">经办人Id</param>
        /// <param name="managerUser">经办人昵称</param>
        /// <param name="orderDate">订单日期</param>
        public PurchaseOrder(long orderId, string orderNo, long managerUserId,
            string managerUser, DateTime orderDate) :
            base(orderId, orderNo, managerUserId, managerUser, orderDate)
        {

        }

        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountTypeEnum? AccountType { get; set; }

        /// <summary>
        /// 供应商|客户Id
        /// </summary>
        public long SupplierClientId { get; set; }

        /// <summary>
        /// 供应商|客户 名
        /// </summary>
        /// <remarks>
        /// 创建订单时的名称，修改后根据供应商|客户Id来
        /// </remarks>
        public string SupplierClientName { get; set; }

        /// <summary>
        /// 订单价格
        /// </summary>
        /// <remarks>
        /// 可能包含商品价格以外的价格
        /// </remarks>
        public decimal OrderPrice { get; protected set; }

        /// <summary>
        /// 订单商品总价
        /// </summary>
        public decimal SumProductPrice { get; protected set; }

        public virtual ICollection<PurchaseOrderDetail>? OrderDetails { get; set; }

        /// <summary>
        /// 计算订单商品总价
        /// </summary>
        public void TotalProductSumPrice()
        {
            if (OrderDetails == null)
            {
                throw new ArgumentException("PurchaseOrder OrderDetails is null");
            }

            SumProductPrice = OrderDetails.Sum(a => a.SumPrice);
            OrderPrice = SumProductPrice;
        }
    }
}
