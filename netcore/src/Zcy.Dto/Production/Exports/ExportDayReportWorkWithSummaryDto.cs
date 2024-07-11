using Ganss.Excel;

namespace Zcy.Dto.Production.Exports
{
    /// <summary>
    /// 导出员工报工汇总
    /// </summary>
    public class ExportDayReportWorkWithSummaryDto
    {
        /// <summary>
        /// 员工用户昵称
        /// </summary>
        [Column("员工名")]
        public string EmployeeNickName { get; set; }

        /// <summary>
        /// 计费类型
        /// </summary>
        [Column("计费类型")]
        public string BillingTypeStr { get; set; }

        /// <summary>
        /// 工作量
        /// </summary>
        /// <remarks>
        ///  数量或时长
        /// </remarks>
        [Column("工作量")]
        public decimal WordDuration { get; set; }
    }
}
