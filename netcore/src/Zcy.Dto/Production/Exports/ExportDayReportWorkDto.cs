using Ganss.Excel;
using Zcy.Entity.Products;
using Zcy.Utility;

namespace Zcy.Dto.Production.Exports
{
    /// <summary>
    /// 导出员工报工 Dto
    /// </summary>
    public class ExportDayReportWorkDto
    {
        /// <summary>
        /// 报工日期
        /// </summary>
        [Column("日期")]
        public string ReportWorkDate { get; set; }

        /// <summary>
        /// 员工用户昵称
        /// </summary>
        [Column("员工名")]
        public string EmployeeNickName { get; set; }

        /// <summary>
        /// 产品名 冗余
        /// </summary>
        [Column("产品名")]
        public string? ProductName { get; set; }

        /// <summary>
        /// 产品工艺名称  冗余
        /// </summary>
        [Column("工艺名")]
        public string? ProductCraftName { get; set; }

        /// <summary>
        /// 计费类型
        /// </summary>
        [Column("计费类型")]
        public string BillingTypeStr => BillingType.GetDisplayName();

        /// <summary>
        /// 工作量
        /// </summary>
        /// <remarks>
        ///  数量或时长
        /// </remarks>
        [Column("工作量")]
        public decimal WordDuration { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("备注")]
        public string? Remark { get; set; }

        /// <summary>
        /// 工艺计费类型 冗余
        /// </summary>
        [Ignore]
        public BillingTypeEnum BillingType { get; set; }
    }
}
