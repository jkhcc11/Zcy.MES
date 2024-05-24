using Zcy.BaseInterface;
using Zcy.Entity.Products;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 查询有效的产品
    /// </summary>
    public class QueryValidProductInput
    {
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
    }
}
