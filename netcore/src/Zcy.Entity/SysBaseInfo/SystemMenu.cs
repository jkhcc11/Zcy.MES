using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.SysBaseInfo
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class SystemMenu : BaseEntity<long>
    {
        /// <summary>
        /// 角色菜单
        /// </summary>
        /// <param name="menuUrl">菜单path</param>
        /// <param name="menuName">菜单名</param>
        /// <param name="routeName">路由名</param>
        public SystemMenu(string menuUrl,
            string menuName, string routeName)
        {
            MenuUrl = menuUrl;
            MenuName = menuName;
            RouteName = routeName;
        }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        public long ParentMenuId { get; protected set; }

        /// <summary>
        /// 菜单Url
        /// </summary>
        public string MenuUrl { get; protected set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string MenuName { get; protected set; }

        /// <summary>
        /// 路由名
        /// </summary>
        /// <remarks>
        ///  这里的名程序需要跟前端页面中的RouteName 一致，否则Cacheable 会失效
        /// </remarks>
        public string RouteName { get; protected set; }

        /// <summary>
        /// icon前缀
        /// </summary>
        public string? IconPrefix { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 是否为根目录
        /// </summary>
        public bool IsRootPath { get; set; }

        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool IsCache { get; set; }

        /// <summary>
        /// 本地文件路径 不包含views
        /// </summary>
        /// <remarks>
        ///  当Menu和文件对不上时 使用这个指定
        ///  /system/subAccount/subAccount-list
        /// </remarks>
        public string? LocalFilePath { get; set; }

        /// <summary>
        /// 排序越大越靠前
        /// </summary>
        public int OrderBy { get; set; }

        public void SetParentMenuId(long parentMenuId)
        {
            ParentMenuId = parentMenuId;
        }

        public void SetMenuUrl(string menuUrl)
        {
            MenuUrl = menuUrl;
        }

        public void SetMenuName(string menuName)
        {
            MenuName = menuName;
        }

        public void SetRouteName(string routeName)
        {
            RouteName = routeName;
        }
    }
}
