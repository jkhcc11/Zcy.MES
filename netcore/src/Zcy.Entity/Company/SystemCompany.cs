using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.Company
{
    /// <summary>
    /// 公司
    /// </summary>
    public class SystemCompany : BaseEntity<long>
    {
        public const int ShorNameLength = 4;

        /// <summary>
        /// 公司
        /// </summary>
        /// <param name="companyName">名</param>
        /// <param name="shortName">缩写</param>
        public SystemCompany(string companyName, string shortName)
        {
            ShortName = shortName;
            CompanyName = companyName;
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
