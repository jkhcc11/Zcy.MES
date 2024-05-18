using Zcy.BaseInterface.BaseModel;

namespace Zcy.Dto.Company
{
    /// <summary>
    /// 查询公司列表
    /// </summary>
    public class QueryPageCompanyDto : BaseFullAuditEntityDto<long>
    {
        public QueryPageCompanyDto(string companyName, string shortName)
        {
            CompanyName = companyName;
            ShortName = shortName;
        }

        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 缩写
        /// </summary>
        /// <remarks>
        /// 一般用于订单号拼接
        /// </remarks>
        public string ShortName { get; set; }

        /// <summary>
        /// 公司抬头
        /// </summary>
        public string? CompanyShowName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
