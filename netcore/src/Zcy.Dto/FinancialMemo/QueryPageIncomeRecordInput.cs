using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Entity.FinancialMemo;

namespace Zcy.Dto.FinancialMemo
{
    /// <summary>
    /// 查询收支记录
    /// </summary>
    public class QueryPageIncomeRecordInput : QueryPageInput, IBaseTimeRangeInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(IncomeRecord.RecordName), ZcyOperator.Like)]
        [ZcyQuery(nameof(IncomeRecord.Remark), ZcyOperator.Like)]
        [ZcyQuery(nameof(IncomeRecord.ManagerUser), ZcyOperator.Like)]
        public string? KeyWord { get; set; }

        /// <summary>
        /// 收支类型
        /// </summary>
        [EnumDataType(typeof(IncomeTypeEnum))]
        [ZcyQuery(nameof(IncomeRecord.IncomeType), ZcyOperator.Equal)]
        public IncomeTypeEnum? IncomeType { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        [EnumDataType(typeof(AccountTypeEnum))]
        [ZcyQuery(nameof(IncomeRecord.AccountType), ZcyOperator.Equal)]
        public AccountTypeEnum? AccountType { get; set; }

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
