using Zcy.BaseInterface;
using Zcy.Entity.SysBaseInfo;

namespace Zcy.Dto.SysBaseInfo
{
    /// <summary>
    /// 查询菜单列表
    /// </summary>
    public class QueryPageMenuInput : QueryPageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [ZcyQuery(nameof(SystemMenu.RouteName), ZcyOperator.Like)]
        [ZcyQuery(nameof(SystemMenu.MenuName), ZcyOperator.Like)]
        [ZcyQuery(nameof(SystemMenu.MenuUrl), ZcyOperator.Like)]
        public string? KeyWord { get; set; }
    }
}
