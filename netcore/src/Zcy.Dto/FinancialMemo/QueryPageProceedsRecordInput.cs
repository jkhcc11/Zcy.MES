using Zcy.BaseInterface;
using Zcy.Entity.FinancialMemo;

namespace Zcy.Dto.FinancialMemo
{
    /// <summary>
    /// 查询收款记录
    /// </summary>
    public class QueryPageProceedsRecordInput : QueryPageInput
    {
        /// <summary>
        /// 客户Id
        /// </summary>
        [ZcyQuery(nameof(ProceedsRecord.SupplierClientId), ZcyOperator.Equal)]
        public long? SupplierClientId { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(ProceedsRecord.Remark), ZcyOperator.Like)]
        public string? KeyWord { get; set; }

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
