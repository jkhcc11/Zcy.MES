using System.ComponentModel.DataAnnotations;
using Zcy.Entity.FinancialMemo;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.FinancialMemo
{
    /// <summary>
    /// 创建收款记录
    /// </summary>
    public class CreateProceedsRecordInput
    {
        public CreateProceedsRecordInput(string proceedsRecordName)
        {
            ProceedsRecordName = proceedsRecordName;
        }

        /// <summary>
        /// 供应商|客户Id
        /// </summary>
        [Range(99999, long.MaxValue)]
        public long SupplierClientId { get; set; }

        /// <summary>
        /// 收款记录名
        /// </summary>
        [Required(ErrorMessage = "收款记录名必填")]
        [StringLength(IncomeRecord.RecordNameLength)]
        public string ProceedsRecordName { get; set; }

        /// <summary>
        /// 记录日期
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "日期格式错误")]
        public DateTime? RecordDate { get; set; }

        /// <summary>
        /// 收款账户类型
        /// </summary>
        [EnumDataType(typeof(AccountTypeEnum))]
        public AccountTypeEnum AccountType { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        [Range(0.01, 999999999999)]
        public decimal Money { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength)]
        public string? Remark { get; set; }
    }
}
