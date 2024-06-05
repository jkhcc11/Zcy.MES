namespace Zcy.Entity.User
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatusEnum : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 离职
        /// </summary>
        Depart = 10,

        /// <summary>
        /// 禁用
        /// </summary>
        Ban = 11
    }
}
