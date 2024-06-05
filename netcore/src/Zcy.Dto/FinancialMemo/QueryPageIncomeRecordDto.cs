using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.FinancialMemo;

namespace Zcy.Dto.FinancialMemo
{
    /// <summary>
    /// 查询收支记录 dto
    /// </summary>
    public class QueryPageIncomeRecordDto : BaseFullAuditEntityDto<long>, IBaseCompanyDto
    {
        public QueryPageIncomeRecordDto(string recordName)
        {
            RecordName = recordName;
        }

        /// <summary>
        /// 收支类型
        /// </summary>
        public IncomeTypeEnum IncomeType { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountTypeEnum AccountType { get; set; }

        /// <summary>
        /// 记录名
        /// </summary>
        public string RecordName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 记录日期
        /// </summary>
        public string RecordDate { get; set; }

        /// <summary>
        /// 经办人
        /// </summary>
        public string? ManagerUser { get; set; }

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
    }
}
