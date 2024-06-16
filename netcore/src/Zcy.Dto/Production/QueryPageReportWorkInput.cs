using Zcy.BaseInterface;
using Zcy.Entity.Production;

namespace Zcy.Dto.Production
{
    /// <summary>
    /// 分页查询报工
    /// </summary>
    public class QueryPageReportWorkInput : QueryPageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(ReportWork.Remark), ZcyOperator.Like)]
        [ZcyQuery(nameof(ReportWork.ProductName), ZcyOperator.Like)]
        [ZcyQuery(nameof(ReportWork.ProductCraftName), ZcyOperator.Like)]
        public string? KeyWord { get; set; }

        /// <summary>
        /// 员工用户Id
        /// </summary>
        [ZcyQuery(nameof(ReportWork.EmployeeId), ZcyOperator.Equal)]
        public long? EmployeeId { get; set; }

        /// <summary>
        /// 产品工序Id
        /// </summary>
        [ZcyQuery(nameof(ReportWork.ProductProcessId), ZcyOperator.Equal)]
        public long? ProductProcessId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
