﻿using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.FinancialMemo;

namespace Zcy.Dto.FinancialMemo
{
    /// <summary>
    /// 查询收款记录
    /// </summary>
    public class QueryPageProceedsRecordDto : BaseFullAuditEntityDto<long>, IBaseCompanyDto, IBaseSupplierClientDto
    {
        /// <summary>
        /// 供应商|客户Id
        /// </summary>
        public long SupplierClientId { get; set; }

        public string? SupplierClientName { get; set; }

        /// <summary>
        /// 收款记录名
        /// </summary>
        public string ProceedsRecordName { get; set; }

        /// <summary>
        /// 记录日期
        /// </summary>
        public string RecordDate { get; set; }

        /// <summary>
        /// 收款账户类型
        /// </summary>
        public AccountTypeEnum AccountType { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }

        public string? CompanyName { get; set; }
    }
}
