using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.Company;

namespace Zcy.Dto.Company
{
    /// <summary>
    /// 查询供应商客户信息列表 dto
    /// </summary>
    public class QueryPageSupplierClientDto : BaseFullAuditEntityDto<long>, IBaseCompanyDto
    {
        public QueryPageSupplierClientDto(string clientName)
        {
            ClientName = clientName;
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 客户状态
        /// </summary>
        public PublicStatusEnum ClientStatus { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public ClientTypeEnum ClientType { get; set; }

        /// <summary>
        ///  联系方式
        /// </summary>
        public string? PhoneNumber { get; set; }

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
