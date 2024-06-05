using System;
using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.FinancialMemo
{
    /// <summary>
    /// 收支记录
    /// </summary>
    public class IncomeRecord : BaseEntity<long>, IBaseCompany
    {
        /// <summary>
        /// 记录名长度
        /// </summary>
        public const int RecordNameLength = 20;

        /// <summary>
        /// 收支记录
        /// </summary>
        /// <param name="incomeType">收支类型</param>
        /// <param name="accountType">账户类型</param>
        /// <param name="recordName">记录名</param>
        /// <param name="money">金额</param>
        /// <param name="recordDate">金额</param>
        /// <param name="managerUser">经办人</param>
        public IncomeRecord(IncomeTypeEnum incomeType, AccountTypeEnum accountType,
            string recordName, decimal money, DateTime recordDate, string managerUser)
        {
            IncomeType = incomeType;
            AccountType = accountType;
            RecordName = recordName;

            var tempMoney = money;
            if (incomeType == IncomeTypeEnum.Out)
            {
                tempMoney = -1 * money;
            }

            Money = tempMoney;
            RecordDate = recordDate;
            ManagerUser = managerUser;
        }

        /// <summary>
        /// 收支类型
        /// </summary>
        public IncomeTypeEnum IncomeType { get; protected set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountTypeEnum AccountType { get; protected set; }

        /// <summary>
        /// 记录名
        /// </summary>
        public string RecordName { get; protected set; }

        /// <summary>
        /// 金额
        /// </summary>
        /// <remarks>
        /// 进账 为+
        /// 出账 为-
        /// </remarks>
        public decimal Money { get; protected set; }

        /// <summary>
        /// 记录日期
        /// </summary>
        public DateTime RecordDate { get; protected set; }

        /// <summary>
        /// 经办人
        /// </summary>
        public string ManagerUser { get; protected set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }
    }
}
