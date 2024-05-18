using Zcy.BaseInterface;

namespace Zcy.Dto.Company
{
    /// <summary>
    /// 查询公司列表
    /// </summary>
    public class QueryPageCompanyInput : QueryPageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? KeyWord { get; set; }
    }
}
