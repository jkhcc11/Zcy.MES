using System.ComponentModel.DataAnnotations;

namespace Zcy.Entity.Products
{
    /// <summary>
    /// 计费类型
    /// </summary>
    public enum BillingTypeEnum : byte
    {
        /// <summary>
        /// 计时
        /// </summary>
        [Display(Name = "计时")]
        Timing = 1,

        /// <summary>
        /// 计件
        /// </summary>
        [Display(Name = "计件")]
        Counting
    }
}
