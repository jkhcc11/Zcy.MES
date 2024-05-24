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
        [ZcyQuery(nameof(ReportWork.Remark), ZcyOperator.Equal)]
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
        /// 报工日期
        /// </summary>
        [ZcyQuery(nameof(ReportWork.ReportWorkDate), ZcyOperator.Equal)]
        public DateTime? ReportWorkDate { get; set; }

    }
}
