using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Entity.Company;
using Zcy.Entity.Production;

namespace Zcy.Dto.Production
{
    /// <summary>
    /// 分页查询报工
    /// </summary>
    public class QueryPageReportWorkInput : QueryPageInput, IBaseTimeRangeInput
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
        /// 报工状态
        /// </summary>
        [ZcyQuery(nameof(ReportWork.ReportWorkStatus), ZcyOperator.Equal)]
        [EnumDataType(typeof(PublicStatusEnum))]
        public PublicStatusEnum? ReportWorkStatus { get; set; }

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
