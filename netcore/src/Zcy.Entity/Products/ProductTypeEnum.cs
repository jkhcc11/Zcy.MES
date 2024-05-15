namespace Zcy.Entity.Products
{
    /// <summary>
    /// 产品类型
    /// </summary>
    public enum ProductTypeEnum : byte
    {
        /// <summary>
        /// 通用件
        /// </summary>
        Common = 1,

        /// <summary>
        /// 原料
        /// </summary>
        Raw = 2,

        /// <summary>
        /// 加工产品
        /// </summary>
        Processing = 3,

        /// <summary>
        /// 销售产品
        /// </summary>
        Sale = 4,
    }
}
