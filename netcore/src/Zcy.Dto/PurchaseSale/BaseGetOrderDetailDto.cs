using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 获取订单详情 基类
    /// </summary>
    public abstract class BaseGetOrderDetailDto : BaseFullAuditEntityDto<long>, IBaseCompanyDto
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNo { get; set; }

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
        public virtual string OrderDate { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public virtual long CompanyId { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// 订单产品总个数
        /// </summary>
        public virtual int OrderProductCount { get; set; }
    }

    /// <summary>
    /// 获取订单详情item
    /// </summary>
    public abstract class BaseGetOrderDetailItemDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual long OrderId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Count { get; protected set; }

        #region 产品冗余
        /// <summary>
        /// 产品名
        /// </summary>
        public virtual string? ProductName { get; set; }

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

    }
}
