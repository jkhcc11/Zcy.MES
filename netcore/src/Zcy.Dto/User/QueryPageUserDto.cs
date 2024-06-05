using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.User;

namespace Zcy.Dto.User
{
    /// <summary>
    /// 分页查询用户 dto
    /// </summary>
    public class QueryPageUserDto : BaseFullAuditEntityDto<long>, IBaseCompanyDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserNick { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string? UserPhone { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string? UserNo { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        /// <remarks>
        /// 超管Id为0
        /// </remarks>
        public long CompanyId { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// 是否启用登录
        /// </summary>
        /// <remarks>
        ///  普通员工也是用户（后面可登录系统 报工等）
        /// </remarks>
        public bool IsEnableLogin { get; set; }

        /// <summary>
        /// 结算基数
        /// </summary>
        /// <remarks>
        ///  员工的薪资结算基数
        /// </remarks>
        public decimal BaseSettlement { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatusEnum UserStatus { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<string> Roles { get; set; }
        public long[]? RoleIds { get; set; }
    }
}
