namespace Zcy.Dto.SysBaseInfo
{
    /// <summary>
    /// 获取角色已激活菜单 dto
    /// </summary>
    public class GetRoleActivateMenuDto
    {
        public GetRoleActivateMenuDto(string menuName)
        {
            MenuName = menuName;
        }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        public long ParentMenuId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string MenuName { get; set; }
    }
}
