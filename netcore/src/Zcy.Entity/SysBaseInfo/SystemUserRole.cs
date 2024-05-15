using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.SysBaseInfo
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class SystemUserRole : BaseEntity<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
    }
}
