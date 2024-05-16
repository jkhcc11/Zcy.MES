using Zcy.BaseInterface.Entities;
using Zcy.Utility;

namespace Zcy.Entity.User
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class SystemUser : BaseEntity<long>, IBaseCompany
    {
        private const string DefaultPwd = "123456";

        /// <summary>
        /// 系统用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userNick">昵称</param>
        public SystemUser(string userName, string userNick)
        {
            UserName = userName;
            UserNick = userNick;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; protected set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserNick { get; protected set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        /// <remarks>
        /// 密文
        /// </remarks>
        public string? UserPwd { get; protected set; }

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
        /// 是否启用登录
        /// </summary>
        /// <remarks>
        ///  普通员工也是用户（后面可登录系统 报工等）
        /// </remarks>
        public bool IsEnableLogin { get; protected set; }

        /// <summary>
        /// 结算基数
        /// </summary>
        /// <remarks>
        ///  员工的薪资结算基数
        /// </remarks>
        public decimal BaseSettlement { get; set; }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="normalPwd">未加密密码</param>
        public void ModifyPwd(string normalPwd)
        {
            UserPwd = normalPwd.ToMd5();
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        public void ModifyNick(string newNick)
        {
            UserNick = newNick;
        }

        /// <summary>
        /// 启用登录
        /// </summary>
        /// <remarks>
        ///  用户密码为空时，并把密码设置为默认密码
        /// </remarks>
        public void EnableLogin()
        {
            if (string.IsNullOrEmpty(UserPwd))
            {
                SetDefaultPwd();
            }

            IsEnableLogin = true;
        }

        /// <summary>
        /// 禁用登录
        /// </summary>
        public void DisableLogin()
        {
            IsEnableLogin = false;
        }

        /// <summary>
        /// 设置默认密码
        /// </summary>
        public void SetDefaultPwd()
        {
            UserPwd = DefaultPwd.ToMd5();
        }

        /// <summary>
        /// 密码是否正确
        /// </summary>
        /// <param name="normalPwd">未加密密码</param>
        /// <returns></returns>
        public bool PwdIsOk(string normalPwd)
        {
            if (string.IsNullOrEmpty(UserPwd))
            {
                return false;
            }

            var tempPwd = normalPwd.ToMd5();
            return tempPwd == UserPwd;
        }
    }
}
