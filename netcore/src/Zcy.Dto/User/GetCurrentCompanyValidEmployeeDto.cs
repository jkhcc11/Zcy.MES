using Zcy.BaseInterface.Entities;

namespace Zcy.Dto.User
{
    /// <summary>
    /// 获取当前公司所有员工列表
    /// </summary>
    public class GetCurrentCompanyValidEmployeeDto : BaseEntity<long>
    {
        public GetCurrentCompanyValidEmployeeDto(string userName, string userNick)
        {
            UserName = userName;
            UserNick = userNick;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserNick { get; set; }
    }
}
