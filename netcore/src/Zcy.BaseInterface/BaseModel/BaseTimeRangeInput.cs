using System;

namespace Zcy.BaseInterface.BaseModel
{
    public interface IBaseTimeRangeInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        DateTime? EndTime { get; set; }
    }

    public static class BaseTimeRangeInputExt
    {
        /// <summary>
        /// 获取日期范围
        /// </summary>
        /// <returns></returns>
        public static (DateTime sTime, DateTime eTime) GetTimeRange(IBaseTimeRangeInput input)
        {
            var sTime = DateTime.Today.AddDays(-30);
            var eTime = DateTime.Today.AddDays(1);

            if (input.StartTime.HasValue)
            {
                sTime = input.StartTime.Value;
            }

            if (input.EndTime.HasValue)
            {
                eTime = input.EndTime.Value;
            }

            if ((eTime - sTime).TotalDays > 366)
            {
                throw new ZcyCustomException("时间范围错误，最大一年范围");
            }

            return (sTime, eTime);
        }
    }
}
