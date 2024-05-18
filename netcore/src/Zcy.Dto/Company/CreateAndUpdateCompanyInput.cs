using System.ComponentModel.DataAnnotations;

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
        public long? Id { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        [Required(ErrorMessage = "公司名必填")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 缩写
        /// </summary>
        /// <remarks>
        /// 一般用于订单号拼接
        /// </remarks>
        [Required(ErrorMessage = "公司缩写必填")]
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
