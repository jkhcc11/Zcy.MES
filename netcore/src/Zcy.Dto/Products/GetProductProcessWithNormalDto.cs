using Zcy.Entity.Products;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 普通用户-获取产品工序
    /// </summary>
    public class GetProductProcessWithNormalDto
    {
        /// <summary>
        /// 产品工序Id
        /// </summary>
        public long ProductProcessId { get; set; }

        /// <summary>
        /// 工艺名
        /// </summary>
        public string CraftName { get; set; }

        /// <summary>
        /// 工艺计费类型
        /// </summary>
        public BillingTypeEnum BillingType { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <remarks>
        /// 越大越靠前
        /// </remarks>
        public int OrderBy { get; set; }

    }
}
