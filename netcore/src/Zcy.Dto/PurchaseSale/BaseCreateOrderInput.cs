using System.ComponentModel.DataAnnotations;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 创建订单 基础input
    /// </summary>
    public abstract class BaseCreateOrderInput
    {
        /// <summary>
        /// 经办人Id
        /// </summary>
        [Range(9999999, long.MaxValue, ErrorMessage = "经办人参数错误")]
        public virtual long? ManagerUserId { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength, ErrorMessage = "订单备注参数错误")]
        public virtual string? OrderRemark { get; set; }

        /// <summary>
        /// 订单日期
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "订单日期错误")]
        public virtual DateTime? OrderDate { get; set; }
    }

    /// <summary>
    /// 创建订单详情 基础input
    /// </summary>
    public abstract class BaseCreateOrderDetailInput
    {
        /// <summary>
        /// 产品Id
        /// </summary>
        [Range(9999999, long.MaxValue, ErrorMessage = "订单产品参数错误")]
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "订单产品数量参数")]
        public virtual int Count { get; set; }
    }
}
