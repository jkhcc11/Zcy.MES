using static Zcy.BaseInterface.AuthorizationConst;
using System.ComponentModel.DataAnnotations;
using Zcy.Entity.SysBaseInfo;

namespace Zcy.Dto.SysBaseInfo
{
    /// <summary>
    /// 创建|修改角色
    /// </summary>
    public class CreateAndUpdateRoleInput
    {
        public CreateAndUpdateRoleInput(string roleName, string roleShowName)
        {
            RoleName = roleName;
            RoleShowName = roleShowName;
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Range(9999999, long.MaxValue)]
        public long? Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        [StringLength(EntityConst.CommonName)]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色显示名
        /// </summary>
        [StringLength(EntityConst.CommonName)]
        public string RoleShowName { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength)]
        public string? Remark { get; set; }
    }
}
