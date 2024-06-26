using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;

namespace Zcy.Dto.DataReport
{
    /// <summary>
    /// 分页查询产品销售报表
    /// </summary>
    public class QueryPageProductSaleReportInput : QueryPageInput, IBaseTimeRangeInput
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
    }
}
