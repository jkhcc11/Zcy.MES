using IdentityModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Utility;

namespace Zcy.MES.HttpService
{
    /// <summary>
    /// 登录信息 实现
    /// </summary>
    public class LoginUserInfo : ILoginUserInfo
    {
        public LoginUserInfo(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext == null)
            {
                return;
            }

            InitUserInfo(httpContextAccessor.HttpContext);
        }

        public bool IsLogin { get; set; }

        public string? UserNick { get; set; }

        public string? UserName { get; set; }

        public long? UserId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string? UserNo { get; set; }

        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        /// <remarks>
        /// 超管Id为0
        /// </remarks>
        public long CompanyId { get; set; }

        public long GetUserId()
        {
            if (UserId.HasValue == false)
            {
                throw new ZcyCustomException("UserInfo is null");
            }

            return UserId.Value;
        }

        public List<string> Roles { get; set; }

        /// <summary>
        /// 是否Boss
        /// </summary>
        public bool IsBoss { get; set; }

        /// <summary>
        /// 从当前请求初始化登录信息
        /// </summary>
        internal void InitUserInfo(HttpContext httpContext)
        {
            var user = httpContext.User;
            if (user.Identity?.IsAuthenticated == false)
            {
                return;
            }

            var subStr = user.Claims.FirstOrDefault(a => a.Type == JwtClaimTypes.Subject)?.Value;
            if (string.IsNullOrEmpty(subStr))
            {
                return;
            }

            Roles = user.Claims
                 .Where(a => a.Type == JwtClaimTypes.Role)
                 .Select(a => a.Value)
                 .ToList();

            IsLogin = true;
            UserId = long.Parse(subStr);
            CompanyId = user.Claims
                .FirstOrDefault(a => a.Type == AuthorizationConst.ZcyClaimTypes.CompanyId)?.Value
                .TryToInt64() ?? 0;
            UserNo = user.Claims.FirstOrDefault(a => a.Type == AuthorizationConst.ZcyClaimTypes.UserNo)?.Value;
            UserName = user.Claims.FirstOrDefault(a => a.Type == JwtClaimTypes.Name)?.Value;
            UserNick = user.Claims.FirstOrDefault(a => a.Type == JwtClaimTypes.NickName)?.Value;
            IsSuperAdmin = user.HasClaim(a => a is
            {
                Type: JwtClaimTypes.Role,
                Value: AuthorizationConst.NormalRoleName.SuperAdmin
            });
            IsBoss = user.HasClaim(a => a is
            {
                Type: JwtClaimTypes.Role,
                Value: AuthorizationConst.NormalRoleName.Boss
            });
            //httpContext.Request.Headers.TryGetValue("Authorization", out var loginToken);
            //LoginToken = loginToken + "";
        }
    }
}
