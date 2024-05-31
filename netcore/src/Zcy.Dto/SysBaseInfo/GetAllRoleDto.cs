namespace Zcy.Dto.SysBaseInfo
{
    /// <summary>
    /// 获取角色列表
    /// </summary>
    public class GetAllRoleDto
    {
        public GetAllRoleDto(string roleName)
        {
            RoleName = roleName;
        }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 角色显示名
        /// </summary>
        public string? RoleShowName { get; set; }
    }
}
