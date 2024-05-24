using System.ComponentModel.DataAnnotations;
using Zcy.Entity.FinancialMemo;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.FinancialMemo
{
    /// <summary>
    /// 创建收支记录
    /// </summary>
    public class CreateIncomeRecordInput
    {
        public CreateIncomeRecordInput(string recordName)
        {
            RecordName = recordName;
        }

        /// <summary>
        /// 收支类型
        /// </summary>
        [EnumDataType(typeof(IncomeTypeEnum))]
        public IncomeTypeEnum IncomeType { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        [EnumDataType(typeof(AccountTypeEnum))]
        public AccountTypeEnum AccountType { get; set; }

        /// <summary>
        /// 记录名
        /// </summary>
        [Required(ErrorMessage = "记录名必填")]
        public string RecordName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 记录日期
        /// </summary>
        /// <remarks>
        /// 默认当天
        /// </remarks>
        public DateTime? RecordDate { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        public string? ManagerUser { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength)]
        public string? Remark { get; set; }
    }
}
