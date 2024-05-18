using Zcy.BaseInterface;

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
        public string? KeyWord { get; set; }
    }
}
