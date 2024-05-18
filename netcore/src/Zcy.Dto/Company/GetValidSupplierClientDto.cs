using Zcy.BaseInterface.BaseModel;

namespace Zcy.Dto.Company
{
    /// <summary>
    /// 获取已启用的客户信息
    /// </summary>
    public class GetValidSupplierClientDto : BaseEntityDto<long>
    {
        public GetValidSupplierClientDto(string clientName)
        {
            ClientName = clientName;
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string ClientName { get; set; }
    }
}
