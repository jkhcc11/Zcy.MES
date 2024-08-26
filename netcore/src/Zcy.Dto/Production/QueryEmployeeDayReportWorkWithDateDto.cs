using Zcy.Entity.Company;
using Zcy.Entity.Products;

namespace Zcy.Dto.Production
{
    /// <summary>
    /// 员工报工记录（无分页，前端打印使用）
    /// </summary>
    public class QueryEmployeeDayReportWorkWithDateDto
    {
        /// <summary>
        /// 员工报工记录
        /// </summary>
        /// <param name="employeeNickName">员工名</param>
        public QueryEmployeeDayReportWorkWithDateDto(string employeeNickName)
        {
            EmployeeNickName = employeeNickName;
        }

        /// <summary>
        /// 员工名
        /// </summary>
        public string EmployeeNickName { get; set; }

        /// <summary>
        /// 表头标题
        /// </summary>
        public string? HeadTitle { get; set; }

        /// <summary>
        /// 表头
        /// </summary>
        public List<string> TableTitleArray { get; set; } = new();

        /// <summary>
        /// 报工行数据
        /// </summary>
        public List<TableRowsItem> TableRowsItems { get; set; } = new();
    }

    /// <summary>
    /// 报工记录item
    /// </summary>
    public class EmployeeDayReportWorkWithDateItem
    {
        /// <summary>
        /// 报工日期
        /// </summary>
        public DateTime ReportWorkDate { get; set; }

        /// <summary>
        /// 员工用户Id
        /// </summary>
        public long EmployeeId { get; set; }

        /// <summary>
        /// 员工用户昵称
        /// </summary>
        public string EmployeeNickName { get; set; }

        /// <summary>
        /// 产品工序Id
        /// </summary>
        public long ProductProcessId { get; set; }

        /// <summary>
        /// 工作量
        /// </summary>
        /// <remarks>
        ///  数量或时长
        /// </remarks>
        public decimal WordDuration { get; set; }

        /// <summary>
        /// 应结算价格
        /// </summary>
        public decimal ReceivableSettlementPrice { get; set; }

        /// <summary>
        /// 实结算价格
        /// </summary>
        public decimal ActualSettlementPrice { get; set; }

        #region 冗余
        /// <summary>
        /// 加工价 冗余
        /// </summary>
        /// <remarks>
        /// 计算工资使用
        /// </remarks>
        public decimal ProcessingPrice { get; set; }

        /// <summary>
        /// 工艺价格 冗余
        /// </summary>
        /// <remarks>
        /// 基础价格，如果有根据产品+工序的在产品选择工序后加
        /// </remarks>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 工艺计费类型 冗余
        /// </summary>
        public BillingTypeEnum BillingType { get; set; }

        /// <summary>
        /// 产品名 冗余
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// 产品工艺名称  冗余
        /// </summary>
        public string? ProductCraftName { get; set; }

        #endregion

    }

    /// <summary>
    /// 报工行数据Item
    /// </summary>
    public class TableRowsItem
    {
        /// <summary>
        /// 列数据
        /// </summary>
        public List<object> ColumnItems { get; set; } = new();
    }
}
