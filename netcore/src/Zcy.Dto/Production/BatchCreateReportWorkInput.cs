using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;

namespace Zcy.Dto.Production
{
    /// <summary>
    /// 批量创建报工
    /// </summary>
    public class BatchCreateReportWorkInput
    {
        /// <summary>
        /// 报工日期
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "报工日期错误")]
        public DateTime? ReportWorkDate { get; set; }

        /// <summary>
        /// 员工用户Id
        /// </summary>
        [Range(9999999, long.MaxValue, ErrorMessage = "员工Id参数错误")]
        public long EmployeeId { get; set; }

        /// <summary>
        /// 报工列表
        /// </summary>
        public List<BatchCreateReportWorkItem> ReportWorkItems { get; set; } = new();
    }

    public class BatchCreateReportWorkItem
    {
        /// <summary>
        /// 产品工序Id
        /// </summary>
        [Range(9999999, long.MaxValue, ErrorMessage = "产品工序Id参数错误")]
        public long ProductProcessId { get; set; }

        /// <summary>
        /// 工作时长
        /// </summary>
        /// <remarks>
        ///  数量或时长
        /// </remarks>
        [Range(0.01, 99999999)]
        public decimal WordDuration { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(AuthorizationConst.EntityConst.BaseRemarkLength)]
        public string? Remark { get; set; }
    }
}
