using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;

namespace Zcy.Dto.DataReport
{
    /// <summary>
    /// 分页查询产品销售报表
    /// </summary>
    public class QueryPageProductSaleReportInput : QueryPageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? KeyWord { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结算时间
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime? EndTime { get; set; }

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
