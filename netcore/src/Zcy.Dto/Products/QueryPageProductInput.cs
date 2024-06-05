using Zcy.BaseInterface;
using Zcy.Entity.Company;
using Zcy.Entity.Products;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 分页查询产品
    /// </summary>
    public class QueryPageProductInput : QueryPageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(Product.ProductName), ZcyOperator.Like)]
        [ZcyQuery(nameof(Product.ProductRemark), ZcyOperator.Like)]
        public string? KeyWord { get; set; }

        /// <summary>
        /// 产品分类Id
        /// </summary>
        [ZcyQuery(nameof(Product.ProductTypeId), ZcyOperator.Equal)]
        public long? ProductTypeId { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        [ZcyQuery(nameof(Product.ProductType), ZcyOperator.Equal)]
        public ProductTypeEnum? ProductType { get; set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        [ZcyQuery(nameof(Product.ProductStatus), ZcyOperator.Equal)]
        public PublicStatusEnum? ProductStatus { get; set; }

        /// <summary>
        /// 是否散件
        /// </summary>
        /// <remarks>
        ///  散件 -> 没规格
        ///  非散件 -> 有规格
        /// </remarks>
        [ZcyQuery(nameof(Product.IsLoose), ZcyOperator.Equal)]
        public bool? IsLoose { get; set; }
    }
}
