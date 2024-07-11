namespace Zcy.Entity.Company
{
    /// <summary>
    /// 通用状态
    /// </summary>
    public enum PublicStatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 待处理
        /// </summary>
        Pending = 2,

        /// <summary>
        /// 禁用
        /// </summary>
        Ban = 5,

        /// <summary>
        /// 驳回
        /// </summary>
        Reject = 6,
    }
}
