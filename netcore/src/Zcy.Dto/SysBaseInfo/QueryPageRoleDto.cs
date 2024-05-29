using Zcy.BaseInterface.BaseModel;

namespace Zcy.Dto.SysBaseInfo
{
    /// <summary>
    /// 分页查询角色 dto
    /// </summary>
    public class QueryPageRoleDto : BaseFullAuditEntityDto<long>
    {
        public QueryPageRoleDto(string roleName, string roleShowName)
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
