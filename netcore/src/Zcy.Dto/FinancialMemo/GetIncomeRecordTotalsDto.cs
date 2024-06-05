using Zcy.Entity.FinancialMemo;

namespace Zcy.Dto.FinancialMemo
{
    /// <summary>
    /// 获取收支记录汇总
    /// </summary>
    public class GetIncomeRecordTotalsDto
    {
        /// <summary>
        /// 进账金额
        /// </summary>
        public decimal InMoney { get; set; }

        /// <summary>
        /// 出账金额
        /// </summary>
        public decimal OutMoney { get; set; }

        /// <summary>
        /// 盈亏
        /// </summary>
        public decimal ProfitMoney => InMoney + OutMoney;

        /// <summary>
        /// 账号类型列表
        /// </summary>
        public List<AccountDetailItem> AccountDetailList { get; set; } = new();
    }

    /// <summary>
    /// 账号类型Item
    /// </summary>
    public class AccountDetailItem
    {
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountTypeEnum AccountType { get; set; }

        /// <summary>
        /// 进账金额
        /// </summary>
        public decimal InMoney { get; set; }

        /// <summary>
        /// 出账金额
        /// </summary>
        public decimal OutMoney { get; set; }

        /// <summary>
        /// 盈亏
        /// </summary>
        public decimal ProfitMoney => InMoney + OutMoney;
    }
}
