using System.Collections.Generic;
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
        public PurchaseOrder(long orderId, string orderNo, long managerUserId,
            string managerUser) : base(orderId, orderNo, managerUserId, managerUser)
        {

        }

        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountTypeEnum? AccountType { get; set; }

        /// <summary>
        /// 供应商|客户Id
        /// </summary>
        public virtual long SupplierClientId { get; set; }

        /// <summary>
        /// 采购总价
        /// </summary>
        public decimal SumPurchasePrice { get; protected set; }

        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        /// <summary>
        /// 设置采购总价
        /// </summary>
        public void SetSumSalePrice(decimal sumPurchasePrice)
        {
            SumPurchasePrice = sumPurchasePrice;
        }
    }
}
