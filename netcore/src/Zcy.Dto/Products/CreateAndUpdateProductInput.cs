﻿using System.ComponentModel.DataAnnotations;
using Zcy.Entity.Products;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 创建|更新 产品
    /// </summary>
    public class CreateAndUpdateProductInput
    {
        public long? Id { get; set; }

        /// <summary>
        /// 产品分类Id
        /// </summary>
        [Range(9999999, long.MaxValue, ErrorMessage = "产品分类参数错误")]
        public long ProductTypeId { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        [Required(ErrorMessage = "产品名必填")]
        [StringLength(Product.ProductNameLength)]
        public string ProductName { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        [EnumDataType(typeof(ProductTypeEnum))]
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
        [Required(ErrorMessage = "单位必填")]
        [StringLength(Product.UnitLength)]
        public string Unit { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        /// <remarks>
        ///  箱/盒/袋
        /// </remarks>
        [StringLength(Product.UnitLength)]
        public string? Spec { get; set; }

        /// <summary>
        /// 规格数
        /// </summary>
        /// <remarks>
        /// 一箱10kg/1盒10个/1袋2kg
        /// </remarks>
        [Range(0, 9999999)]
        public int? SpecCount { get; set; }

        /// <summary>
        /// 产品备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength)]
        public string? ProductRemark { get; set; }

        /// <summary>
        /// 产品工序
        /// </summary>
        public List<ProductProcessItem>? ProductProcesses { get; set; }
    }

    /// <summary>
    /// 产品工艺Item
    /// </summary>
    public class ProductCraftItem
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

        /// <summary>
        /// 工艺价格
        /// </summary>
        public decimal CraftPrice { get; set; }
    }

    /// <summary>
    /// 产品工序Item
    /// </summary>
    public class ProductProcessItem
    {
        /// <summary>
        /// 产品工序Id
        /// </summary>
        public long ProductProcessId { get; set; }

        /// <summary>
        /// 产品工艺
        /// </summary>
        public ProductCraftItem? ProductCraft { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>
        /// 越大越靠前
        /// </remarks>
        [Range(0, 999, ErrorMessage = "排序错误")]
        public int OrderBy { get; set; }

        /// <summary>
        /// 工艺Id
        /// </summary>
        [Range(9999999, long.MaxValue, ErrorMessage = "工艺Id参数错误")]
        public long CraftId { get; set; }

        /// <summary>
        /// 加工价
        /// </summary>
        /// <remarks>
        /// 计算工资使用
        /// </remarks>
        [Range(0, 99999999, ErrorMessage = "加工价参数错误")]
        public decimal ProcessingPrice { get; set; }

        /// <summary>
        /// 工序计费价
        /// </summary>
        public decimal SumPrice => (ProductCraft?.CraftPrice ?? 0) + ProcessingPrice;
    }
}
