using Zcy.BaseInterface;

namespace Zcy.Dto.User
{
    /// <summary>
    /// 分页查询用户
    /// </summary>
    public class QueryPageUserInput : QueryPageInput
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? KeyWord { get; set; }
    }
}
