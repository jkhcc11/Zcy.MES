using System;
using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.PurchaseSale
{
    /// <summary>
    /// 基础订单详情
    /// </summary>
    public abstract class BaseOrderDetail : BaseEntity<long>, IBaseCompany
    {
        /// <summary>
        /// 基础订单详情
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <param name="orderDate">订单日期</param>
        /// <param name="productId">产品Id</param>
        /// <param name="count">数量</param>
        protected BaseOrderDetail(long orderId, DateTime orderDate, long productId, int count)
        {
            OrderDate = orderDate;
            OrderId = orderId;
            ProductId = productId;
            Count = count;
        }

        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual long OrderId { get; protected set; }

        /// <summary>
        /// 订单日期(冗余)
        /// </summary>
        public virtual DateTime OrderDate { get; protected set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual long ProductId { get; protected set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Count { get; protected set; }

        #region 产品冗余
        /// <summary>
        /// 产品名
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 是否散件 冗余
        /// </summary>
        /// <remarks>
        ///  散件 -> 没规格
        ///  非散件 -> 有规格
        /// </remarks>
        public virtual bool IsLoose { get; set; }

        /// <summary>
        /// 单位 冗余
        /// </summary>
        /// <remarks>
        ///  kg/个
        /// </remarks>
        public virtual string? Unit { get; set; }

        /// <summary>
        /// 规格 冗余
        /// </summary>
        /// <remarks>
        ///  冗余
        /// </remarks>
        public virtual string? Spec { get; set; }

        /// <summary>
        /// 规格数 冗余
        /// </summary>
        /// <remarks>
        /// 一箱10kg/1盒10个/1袋2kg 冗余
        /// </remarks> 
        public virtual int SpecCount { get; set; }
        #endregion

        /// <summary>
        /// 设置产品冗余信息
        /// </summary>
        /// <param name="productName">产品名</param>
        /// <param name="isLoose">是否散件</param>
        /// <param name="unit">单位</param>
        /// <param name="spec">规格</param>
        /// <param name="specCount">规格数</param>
        public void SetProductInfo(string productName, bool isLoose, string unit,
            string? spec, int specCount)
        {
            ProductName = productName;
            IsLoose = isLoose;
            Unit = unit;
            Spec = spec;
            SpecCount = specCount;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }
    }
}
