namespace Zcy.Entity.FinancialMemo
{
    /// <summary>
    /// 收款记录分组Item
    /// </summary>
    public class ProceedsRecordGroupTemp
    {
        /// <summary>
        /// 收款账户类型
        /// </summary>
        public AccountTypeEnum AccountType { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Money { get; set; }
    }
}
