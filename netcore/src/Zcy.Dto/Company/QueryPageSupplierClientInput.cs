using Zcy.BaseInterface;
using Zcy.Entity.Company;

namespace Zcy.Dto.Company
{
    /// <summary>
    /// 查询供应商客户信息列表
    /// </summary>
    public class QueryPageSupplierClientInput : QueryPageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(SupplierClient.PhoneNumber), ZcyOperator.Like)]
        [ZcyQuery(nameof(SupplierClient.ClientName), ZcyOperator.Like)]
        public string? KeyWord { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        [ZcyQuery(nameof(SupplierClient.ClientType), ZcyOperator.Equal)]
        public ClientTypeEnum? ClientType { get; set; }
    }
}
