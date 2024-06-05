using System.ComponentModel.DataAnnotations;
using Zcy.Entity.Products;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 创建|更新 产品分类
    /// </summary>
    public class CreateUpdateProductTypeInput
    {
        public long? Id { get; set; }

        public CreateUpdateProductTypeInput(string typeName)
        {
            TypeName = typeName;
        }

        /// <summary>
        /// 分类名
        /// </summary>
        [Required(ErrorMessage = "分类名必填")]
        [StringLength(ProductType.TypeNameLength)]
        public string TypeName { get; set; }
    }
}
