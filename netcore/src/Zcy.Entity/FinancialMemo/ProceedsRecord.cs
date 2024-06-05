using System;
using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.FinancialMemo
{
    /// <summary>
    /// 收款记录
    /// </summary>
    public class ProceedsRecord : BaseEntity<long>, IBaseCompany
    {
        /// <summary>
        /// 收款记录
        /// </summary>
        /// <param name="proceedsRecordName">收款记录名</param>
        /// <param name="supplierClientId">客户Id</param>
        /// <param name="recordDate">记录日期</param>
        /// <param name="accountType">收款账户类型</param>
        /// <param name="money">收款金额</param>
        public ProceedsRecord(string proceedsRecordName, long supplierClientId, DateTime recordDate,
            AccountTypeEnum accountType, decimal money)
        {
            ProceedsRecordName = proceedsRecordName;
            SupplierClientId = supplierClientId;
            RecordDate = recordDate;
            AccountType = accountType;
            Money = money;
        }

        /// <summary>
        /// 供应商|客户Id
        /// </summary>
        public long SupplierClientId { get; protected set; }

        /// <summary>
        /// 收款记录名
        /// </summary>
        public string ProceedsRecordName { get; protected set; }

        /// <summary>
        /// 记录日期
        /// </summary>
        public DateTime RecordDate { get; protected set; }

        /// <summary>
        /// 收款账户类型
        /// </summary>
        public AccountTypeEnum AccountType { get; protected set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Money { get; protected set; }

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
