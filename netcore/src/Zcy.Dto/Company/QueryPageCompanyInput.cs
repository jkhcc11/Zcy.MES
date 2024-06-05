using Zcy.BaseInterface;
using Zcy.Entity.Company;

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
        [ZcyQuery(nameof(SystemCompany.CompanyName), ZcyOperator.Like)]
        [ZcyQuery(nameof(SystemCompany.ShortName), ZcyOperator.Like)]
        [ZcyQuery(nameof(SystemCompany.CompanyShowName), ZcyOperator.Like)]
        public string? KeyWord { get; set; }
    }
}
