using System.ComponentModel.DataAnnotations;
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
        public long? Id { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Required(ErrorMessage = "客户名称必填")]
        public string ClientName { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        [EnumDataType(typeof(ClientTypeEnum))]
        public ClientTypeEnum ClientType { get; set; }

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
