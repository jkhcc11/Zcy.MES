using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 分页查询订单基类
    /// </summary>
    public abstract class BaseQueryPageOrderDto : BaseFullAuditEntityDto<long>, IBaseCompanyDto
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
    }
}
