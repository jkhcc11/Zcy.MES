﻿using System.ComponentModel.DataAnnotations;
using Zcy.Entity.Company;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.Company
{
    /// <summary>
    /// 创建|更新 供应商客户信息
    /// </summary>
    public class CreateAndUpdateSupplierClientInput
    {
        public CreateAndUpdateSupplierClientInput(string clientName)
        {
            ClientName = clientName;
        }

        /// <summary>
        /// Key
        /// </summary>
        [Range(9999999, long.MaxValue)]
        public long? Id { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        [Range(9999999, long.MaxValue)]
        public long? CompanyId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Required(ErrorMessage = "客户名称必填")]
        [StringLength(EntityConst.CommonName)]
        public string ClientName { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        [EnumDataType(typeof(ClientTypeEnum))]
        public ClientTypeEnum ClientType { get; set; }

        /// <summary>
        /// 客户状态
        /// </summary>
        [EnumDataType(typeof(PublicStatusEnum))]
        public PublicStatusEnum ClientStatus { get; set; }

        /// <summary>
        ///  联系方式
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength)]
        public string? Remark { get; set; }

    }
}
