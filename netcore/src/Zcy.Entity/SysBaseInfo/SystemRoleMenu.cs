using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.SysBaseInfo
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    public class SystemRoleMenu : BaseEntity<long>
    {
        /// <summary>
        /// 角色菜单
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        /// <param name="roleId">角色Id</param>
        /// <param name="isActivate">是否激活</param>
        public SystemRoleMenu(long menuId, long roleId, bool isActivate)
        {
            MenuId = menuId;
            RoleId = roleId;
            IsActivate = isActivate;
        }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; protected set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; protected set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActivate { get; set; }

        public void NotActivated()
        {
            IsActivate = false;
        }

        public void Activated()
        {
            IsActivate = true;
        }
    }
}
