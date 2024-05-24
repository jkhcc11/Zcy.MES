using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.Company;
using Zcy.Entity.Products;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 分页查询产品
    /// </summary>
    public class QueryPageProductDto : BaseFullAuditEntityDto<long>, IBaseCompanyDto
    {
        /// <summary>
        /// 产品分类Id
        /// </summary>
        public long ProductTypeId { get; set; }

        /// <summary>
        /// 产品分类名
        /// </summary>
        public string? ProductTypeName { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductTypeEnum ProductType { get; set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        public PublicStatusEnum ProductStatus { get; set; }

        /// <summary>
        /// 是否散件
        /// </summary>
        /// <remarks>
        ///  散件 -> 没规格
        ///  非散件 -> 有规格
        /// </remarks>
        public bool IsLoose { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        /// <remarks>
        ///  kg/个
        /// </remarks>
        public string Unit { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        /// <remarks>
        ///  箱/盒/袋
        /// </remarks>
        public string? Spec { get; set; }

        /// <summary>
        /// 规格数
        /// </summary>
        /// <remarks>
        /// 一箱10kg/1盒10个/1袋2kg
        /// </remarks>
        public int SpecCount { get; set; }

        /// <summary>
        /// 产品备注
        /// </summary>
        public string? ProductRemark { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName { get; set; }
    }
}
