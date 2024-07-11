using System.Collections.Generic;

namespace Zcy.BaseInterface.Service
{
    /// <summary>
    /// 登录信息 接口
    /// </summary>
    public interface ILoginUserInfo
    {
        /// <summary>
        /// 是否登录
        /// </summary>
        bool IsLogin { set; get; }

        /// <summary>
        /// 昵称
        /// </summary>
        string? UserNick { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        string? UserName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        long? UserId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        string? UserNo { get; set; }

        /// <summary>
        /// 是否超管
        /// </summary>
        bool IsSuperAdmin { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        /// <remarks>
        /// 超管Id为0
        /// </remarks>
        long CompanyId { get; set; }

        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <returns></returns>
        long GetUserId();

        /// <summary>
        /// 获取用户Name
        /// </summary>
        /// <returns></returns>
        string GetUserName();

        /// <summary>
        /// 角色名
        /// </summary>
        List<string> Roles { get; set; }

        /// <summary>
        /// 是否Boss
        /// </summary>
        bool IsBoss { get; set; }

        /// <summary>
        /// 是否非超管或Boss
        /// </summary>
        bool IsNotBossAndRoot { get; set; }

        /// <summary>
        /// 是否普通用户
        /// </summary>
        bool IsNormal { get; set; }
    }
}
