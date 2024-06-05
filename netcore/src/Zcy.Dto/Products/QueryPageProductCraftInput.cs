using System.ComponentModel.DataAnnotations;
using Zcy.BaseInterface;
using Zcy.Entity.Company;
using Zcy.Entity.Products;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 分页查询产品工艺
    /// </summary>
    public class QueryPageProductCraftInput : QueryPageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(ProductCraft.CraftName), ZcyOperator.Like)]
        [ZcyQuery(nameof(ProductCraft.Remark), ZcyOperator.Like)]
        public string? KeyWord { get; set; }

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

        /// <summary>
        /// 工艺状态
        /// </summary>
        [EnumDataType(typeof(PublicStatusEnum))]
        [ZcyQuery(nameof(ProductCraft.CraftStatus), ZcyOperator.Equal)]
        public PublicStatusEnum? CraftStatus { get; set; }
    }
}
