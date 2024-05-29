using Zcy.BaseInterface;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 分页查询订单基类
    /// </summary>
    public abstract class BaseQueryPageOrderInput : QueryPageInput
    {
        /// <summary>
        /// 业务Id
        /// </summary>
        /// <remarks>
        ///  供应商Id|客户Id 等
        /// </remarks>
        public abstract long? BusinessId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public abstract string? KeyWord { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public abstract DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public abstract DateTime? EndTime { get; set; }
    }
}
