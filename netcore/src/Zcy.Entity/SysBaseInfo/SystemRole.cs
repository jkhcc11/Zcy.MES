using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.SysBaseInfo
{
    /// <summary>
    /// 系统角色
    /// </summary>
    public class SystemRole : BaseEntity<long>
    {
        public SystemRole(string roleName, string roleShowName)
        {
            RoleName = roleName;
            RoleShowName = roleShowName;
        }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色显示名
        /// </summary>
        public string RoleShowName { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
