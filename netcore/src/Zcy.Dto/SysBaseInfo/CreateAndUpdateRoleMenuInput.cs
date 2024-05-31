using System.ComponentModel.DataAnnotations;

namespace Zcy.Dto.SysBaseInfo
{
    /// <summary>
    /// 创建和更新角色菜单
    /// </summary>
    public class CreateAndUpdateRoleMenuInput
    {
        public CreateAndUpdateRoleMenuInput(long roleId, List<CreateAndUpdateRoleMenuItem> menuItems)
        {
            RoleId = roleId;
            MenuItems = menuItems;
        }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Range(9999999, long.MaxValue)]
        public long RoleId { get; set; }

        /// <summary>
        /// 菜单Item
        /// </summary>
        public List<CreateAndUpdateRoleMenuItem> MenuItems { get; set; }
    }

    public class CreateAndUpdateRoleMenuItem
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Range(9999999, long.MaxValue)]
        public long MenuId { get; set; }
    }
}
