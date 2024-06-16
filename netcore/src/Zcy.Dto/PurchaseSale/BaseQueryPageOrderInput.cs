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

        /// <summary>
        /// 获取日期返回
        /// </summary>
        /// <returns></returns>
        public virtual (DateTime sTime, DateTime eTime) GetTimeRange()
        {
            var sTime = DateTime.Today.AddDays(-30);
            var eTime = DateTime.Today.AddDays(1);

            if (StartTime.HasValue)
            {
                sTime = StartTime.Value;
            }

            if (EndTime.HasValue)
            {
                eTime = EndTime.Value;
            }

            if ((eTime - sTime).TotalDays > 366)
            {
                throw new ZcyCustomException("时间范围错误，最大一年范围");
            }

            return (sTime, eTime);
        }
    }
}
