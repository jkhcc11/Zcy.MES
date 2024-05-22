using System.Collections.Generic;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.Company;

namespace Zcy.Entity.Products
{
    /// <summary>
    /// 产品
    /// </summary>
    public class Product : BaseEntity<long>, IBaseCompany
    {
        /// <summary>
        /// 产品
        /// </summary>
        /// <param name="productTypeId">产品分类Id</param>
        /// <param name="productName">产品名</param>
        /// <param name="productType">产品类型</param>
        /// <param name="isLoose">是否散件</param>
        /// <param name="unit">单位</param>
        public Product(long productTypeId, string productName, ProductTypeEnum productType,
            bool isLoose, string unit)
        {
            ProductName = productName;
            ProductType = productType;
            IsLoose = isLoose;
            Unit = unit;
            ProductTypeId = productTypeId;
            ProductStatus = PublicStatusEnum.Normal;
        }

        /// <summary>
        /// 产品分类Id
        /// </summary>
        public long ProductTypeId { get; protected set; }

        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName { get; protected set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductTypeEnum ProductType { get; protected set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        public PublicStatusEnum ProductStatus { get; protected set; }

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
        public string Unit { get; protected set; }

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
        /// 产品工序
        /// </summary>
        public virtual ICollection<ProductProcess> ProductProcesses { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public void Ban()
        {
            ProductStatus = PublicStatusEnum.Ban;
        }
    }
}
