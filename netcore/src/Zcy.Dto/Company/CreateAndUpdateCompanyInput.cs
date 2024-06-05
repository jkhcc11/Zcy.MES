using System.ComponentModel.DataAnnotations;
using Zcy.Entity.Company;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.Company
{
    /// <summary>
    /// 创建|修改 公司信息
    /// </summary>
    public class CreateAndUpdateCompanyInput
    {
        public CreateAndUpdateCompanyInput(string companyName, string shortName)
        {
            CompanyName = companyName;
            ShortName = shortName;
        }

        /// <summary>
        /// Key
        /// </summary>
        [Range(9999999, long.MaxValue)]
        public long? Id { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        [Required(ErrorMessage = "公司名必填")]
        [StringLength(EntityConst.CommonName)]
        public string CompanyName { get; set; }

        /// <summary>
        /// 缩写
        /// </summary>
        /// <remarks>
        /// 一般用于订单号拼接
        /// </remarks>
        [Required(ErrorMessage = "公司缩写必填")]
        [StringLength(SystemCompany.ShorNameLength, MinimumLength = 2, ErrorMessage = "公司缩写在2-4位")]
        public string ShortName { get; set; }

        /// <summary>
        /// 公司抬头
        /// </summary>
        [StringLength(EntityConst.CommonName)]
        public string? CompanyShowName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength)]
        public string? Remark { get; set; }
    }
}
