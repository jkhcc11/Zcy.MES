using Zcy.BaseInterface;

namespace Zcy.Dto.SysBaseInfo
{
    /// <summary>
    /// 分页查询角色 input
    /// </summary>
    public class QueryPageRoleInput : QueryPageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? KeyWord { get; set; }
    }
}
