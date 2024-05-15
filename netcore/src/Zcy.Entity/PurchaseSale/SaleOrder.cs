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
        public SaleOrder(long orderId, string orderNo, long managerUserId,
            string managerUser, SaleOrderStatusEnum saleOrderStatus)
            : base(orderId, orderNo, managerUserId, managerUser)
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
        ///  不含运费
        /// </remarks>
        public decimal SumSalePrice { get; protected set; }

        /// <summary>
        /// 订单价格
        /// </summary>
        /// <remarks>
        ///  销售总价+运费价格
        /// </remarks>
        public decimal OrderPrice { get; protected set; }

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
        /// 设置销售总价
        /// </summary>
        public void SetSumSalePrice(decimal sumSalePrice)
        {
            SumSalePrice = sumSalePrice;
            OrderPrice = SumSalePrice + FreightPrice;
        }
    }
}
