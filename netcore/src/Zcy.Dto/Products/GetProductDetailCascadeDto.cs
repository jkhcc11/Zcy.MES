using Zcy.BaseInterface.BaseModel;
using Zcy.Entity.Products;

namespace Zcy.Dto.Products
{
    /// <summary>
    ///  获取产品详情（级联方式）
    /// </summary>
    public class GetProductDetailCascadeDto : BaseEntityDto<long>
    {
        /// <summary>
        /// 产品分类Id
        /// </summary>
        public long ProductTypeId { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductTypeEnum ProductType { get; set; }

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
        public int? SpecCount { get; set; }

        /// <summary>
        /// 产品工序
        /// </summary>
        public List<GetProductDetailCascadeItem>? ProductProcesses { get; set; }
    }

    public class GetProductDetailCascadeItem
    {
        /// <summary>
        /// 产品工序Id
        /// </summary>
        public long ProductProcessId { get; set; }

        /// <summary>
        /// 产品工艺
        /// </summary>
        public ProductDetailCascadeCraftItem? ProductCraft { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>
        /// 越大越靠前
        /// </remarks>
        public int OrderBy { get; set; }

        /// <summary>
        /// 工艺Id
        /// </summary>
        public long CraftId { get; set; }
    }

    public class ProductDetailCascadeCraftItem
    {
        /// <summary>
        /// 工艺Id
        /// </summary>
        public long CraftId { get; set; }

        /// <summary>
        /// 工艺名
        /// </summary>
        public string CraftName { get; set; }

        /// <summary>
        /// 工艺计费类型
        /// </summary>
        public BillingTypeEnum BillingType { get; set; }
    }
}