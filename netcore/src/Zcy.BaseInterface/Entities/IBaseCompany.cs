namespace Zcy.BaseInterface.Entities
{
    /// <summary>
    /// 公司Id基类
    /// </summary>
    public interface IBaseCompany
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }
    }

    public interface IBaseCompanyDto
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string? CompanyName { get; set; }
    }
}
