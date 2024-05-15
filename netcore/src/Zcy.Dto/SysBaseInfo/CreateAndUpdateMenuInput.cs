using System.ComponentModel.DataAnnotations;

namespace Zcy.Dto.SysBaseInfo
{
    /// <summary>
    /// 创建和更新菜单
    /// </summary>
    public class CreateAndUpdateMenuInput
    {
        public CreateAndUpdateMenuInput(string menuUrl, string menuName, string routeName)
        {
            MenuUrl = menuUrl;
            MenuName = menuName;
            RouteName = routeName;
        }

        public long? Id { get; set; }

        /// <summary>
        /// 父菜单Id
        /// </summary>
        public long? ParentMenuId { get; set; }

        /// <summary>
        /// 菜单Url
        /// </summary>
        [Required(ErrorMessage = "菜单不能为空")]
        public string MenuUrl { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        [Required(ErrorMessage = "菜单名不能为空")]
        public string MenuName { get; set; }

        /// <summary>
        /// 路由名
        /// </summary>
        /// <remarks>
        ///  这里的名程序需要跟前端页面中的RouteName 一致，否则Cacheable 会失效
        /// </remarks>
        [Required(ErrorMessage = "路由名不能为空")]
        public string RouteName { get; set; }

        /// <summary>
        /// icon前缀
        /// </summary>
        public string? IconPrefix { get; set; } = "iconfont";

        /// <summary>
        /// icon
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 是否为根目录
        /// </summary>
        public bool? IsRootPath { get; set; }

        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool? IsCache { get; set; }

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
    }
}
