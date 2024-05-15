using Zcy.BaseInterface;

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
        public string? KeyWord { get; set; }
    }
}
