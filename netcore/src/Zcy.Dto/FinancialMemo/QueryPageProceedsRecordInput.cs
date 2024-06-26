using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Entity.FinancialMemo;

namespace Zcy.Dto.FinancialMemo
{
    /// <summary>
    /// 查询收款记录
    /// </summary>
    public class QueryPageProceedsRecordInput : QueryPageInput, IBaseTimeRangeInput
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
        [ZcyQuery(nameof(ProceedsRecord.ProceedsRecordName), ZcyOperator.Like)]
        public string? KeyWord { get; set; }

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
