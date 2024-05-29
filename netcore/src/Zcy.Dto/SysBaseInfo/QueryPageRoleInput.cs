using Zcy.BaseInterface;
using Zcy.Entity.SysBaseInfo;

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
        [ZcyQuery(nameof(SystemRole.RoleName), ZcyOperator.Like)]
        [ZcyQuery(nameof(SystemRole.RoleShowName), ZcyOperator.Like)]
        public string? KeyWord { get; set; }
    }
}
