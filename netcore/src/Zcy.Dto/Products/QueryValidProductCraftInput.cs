using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.Entity.Products;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 查询有效的产品工艺
    /// </summary>
    public class QueryValidProductCraftInput
    {
        /// <summary>
        /// 工艺类型
        /// </summary>
        /// <remarks>
        /// 独立工艺: ■一般为计时 不用配置产品线使用。比如捡货计时|收货计时 <br/>
        /// 加工工艺：■配合产品使用
        /// </remarks>
        [EnumDataType(typeof(CraftTypeEnum))]
        [ZcyQuery(nameof(ProductCraft.CraftType), ZcyOperator.Equal)]
        public CraftTypeEnum? CraftType { get; set; }

        /// <summary>
        /// 计费类型
        /// </summary>
        [EnumDataType(typeof(BillingTypeEnum))]
        [ZcyQuery(nameof(ProductCraft.BillingType), ZcyOperator.Equal)]
        public BillingTypeEnum? BillingType { get; set; }
    }
}
