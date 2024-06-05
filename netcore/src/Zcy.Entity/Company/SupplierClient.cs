using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.Company
{
    /// <summary>
    /// 供应商|客户信息
    /// </summary>
    public class SupplierClient : BaseEntity<long>, IBaseCompany
    {
        /// <summary>
        /// 客户信息
        /// </summary>
        /// <param name="clientName">客户名</param>
        /// <param name="clientType">客户类型</param>
        public SupplierClient(string clientName, ClientTypeEnum clientType)
        {
            ClientName = clientName;
            ClientType = clientType;
            ClientStatus = PublicStatusEnum.Normal;
        }


        /// <summary>
        /// 客户名称
        /// </summary>
        public string ClientName { get; protected set; }

        /// <summary>
        /// 客户状态
        /// </summary>
        public PublicStatusEnum ClientStatus { get; protected set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public ClientTypeEnum ClientType { get; protected set; }

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

        public void SetClientName(string clientName)
        {
            ClientName = clientName;
        }

        /// <summary>
        /// 禁用
        /// </summary>
        public void Ban()
        {
            ClientStatus = PublicStatusEnum.Ban;
        }

        /// <summary>
        /// 启用
        /// </summary>
        public void Open()
        {
            ClientStatus = PublicStatusEnum.Normal;
        }
    }
}
