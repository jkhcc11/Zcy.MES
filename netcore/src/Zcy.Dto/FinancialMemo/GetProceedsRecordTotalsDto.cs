using Zcy.BaseInterface.BaseModel;
using Zcy.Entity.FinancialMemo;

namespace Zcy.Dto.FinancialMemo
{
    /// <summary>
    /// 获取收款记录汇总
    /// </summary>
    public class GetProceedsRecordTotalsDto
    {
        /// <summary>
        /// 总额
        /// </summary>
        public decimal TotalMoney => AccountDetailList.Sum(a => a.Money);

        /// <summary>
        /// 账号类型列表
        /// </summary>
        public List<AccountDetailWithProceedsRecordItem> AccountDetailList { get; set; } = new();
    }

    /// <summary>
    /// 收款 账号类型Item
    /// </summary>
    public class AccountDetailWithProceedsRecordItem
    {
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountTypeEnum AccountType { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
    }
}
