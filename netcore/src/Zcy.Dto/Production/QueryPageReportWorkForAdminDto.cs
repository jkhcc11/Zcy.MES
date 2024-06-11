using Zcy.BaseInterface.BaseModel;
using Zcy.Entity.Products;

namespace Zcy.Dto.Production
{
    /// <summary>
    /// 分页查询报工(管理员)
    /// </summary>
    public class QueryPageReportWorkForAdminDto : BaseFullAuditEntityDto<long>
    {
        /// <summary>
        /// 报工日期
        /// </summary>
        public string ReportWorkDate { get; set; }

        /// <summary>
        /// 员工用户Id
        /// </summary>
        public long EmployeeId { get; set; }

        public string? EmployeeName { get; set; }

        /// <summary>
        /// 产品工序Id
        /// </summary>
        public long ProductProcessId { get; set; }

        /// <summary>
        /// 工作时长
        /// </summary>
        /// <remarks>
        ///  数量或时长
        /// </remarks>
        public decimal WordDuration { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string? CompanyName { get; set; }

        #region 冗余
        /// <summary>
        /// 工艺计费类型 冗余
        /// </summary>
        public BillingTypeEnum BillingType { get; set; }

        /// <summary>
        /// 产品工序名称  冗余
        /// </summary>
        /// <remarks>
        /// 产品名/工序名
        /// </remarks>
        public string? ProductProcessName { get; set; }
        #endregion

    }
}
