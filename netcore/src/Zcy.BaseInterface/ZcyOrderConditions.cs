namespace Zcy.BaseInterface
{
    /// <summary>
    /// EF条件表达式
    /// </summary>
    public class ZcyOrderConditions
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string? Key { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public ZcyOrderByEnum OrderBy { get; set; }
    }

    /// <summary>
    /// 排序
    /// </summary>
    public enum ZcyOrderByEnum
    {
        /// <summary>
        /// 升序
        /// </summary>
        Asc,

        /// <summary>
        /// 降序
        /// </summary>
        Desc
    }
}
