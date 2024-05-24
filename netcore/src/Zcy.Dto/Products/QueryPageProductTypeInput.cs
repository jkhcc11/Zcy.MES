using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.Entity.Company;
using Zcy.Entity.Products;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 分页查询产品分类
    /// </summary>
    public class QueryPageProductTypeInput : QueryPageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(ProductType.TypeName), ZcyOperator.Like)]
        public string? KeyWord { get; set; }

        /// <summary>
        /// 分类状态
        /// </summary>
        [EnumDataType(typeof(PublicStatusEnum))]
        [ZcyQuery(nameof(ProductType.TypeStatus), ZcyOperator.Equal)]
        public PublicStatusEnum? TypeStatus { get; set; }
    }
}
