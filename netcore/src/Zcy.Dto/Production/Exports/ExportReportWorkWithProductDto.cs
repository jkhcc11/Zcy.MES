using Ganss.Excel;

namespace Zcy.Dto.Production.Exports
{
    /// <summary>
    /// 导出产品报工 Dto
    /// </summary>
    public class ExportReportWorkWithProductDto
    {
        /// <summary>
        /// 报工日期
        /// </summary>
        [Column("日期")]
        public string ReportWorkDate { get; set; }

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
        /// 工作量
        /// </summary>
        /// <remarks>
        ///  数量或时长
        /// </remarks>
        [Column("数量/时长")]
        public decimal WordDuration { get; set; }
    }
}
