using System;
using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.PurchaseSale
{
    /// <summary>
    /// 基础订单
    /// </summary>
    public abstract class BaseOrder : BaseEntity<long>, IBaseCompany
    {
        public const int OrderNoLength = 25;

        /// <summary>
        /// 基础订单
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="orderNo">订单号</param>
        /// <param name="managerUserId">经办人Id</param>
        /// <param name="managerUser">经办人昵称</param>
        /// <param name="orderDate">订单日期</param>
        protected BaseOrder(long orderId, string orderNo,
            long managerUserId, string managerUser,
            DateTime orderDate)
        {
            Id = orderId;
            OrderNo = orderNo;
            ManagerUserId = managerUserId;
            ManagerUser = managerUser;
            OrderDate = orderDate;
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNo { get; protected set; }

        /// <summary>
        /// 经办人Id
        /// </summary>
        public virtual long ManagerUserId { get; set; }

        /// <summary>
        /// 经办人昵称
        /// </summary>
        /// <remarks>
        /// 创建订单时的昵称，修改后根据经办人Id来
        /// </remarks>
        public virtual string ManagerUser { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public virtual string? OrderRemark { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        public virtual DateTime OrderDate { get; protected set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public virtual long CompanyId { get; set; }

        /// <summary>
        /// 订单摘要
        /// </summary>
        /// <remarks>
        /// 自动根据产品列表生成
        /// </remarks>
        public virtual string? OrderSummary { get; set; }

        /// <summary>
        /// 订单产品总个数
        /// </summary>
        public virtual int OrderProductCount { get; protected set; }
    }
}
