using Microsoft.Extensions.DependencyInjection;

namespace Zcy.BaseInterface
{

    /// <summary>
    /// 授权常量
    /// </summary>
    public class AuthorizationConst
    {
        /// <summary>
        /// 全局ServiceCollection 谨慎使用
        /// </summary>
        public static IServiceCollection ServiceCollection { get; set; }

        /// <summary>
        /// 通用策略名
        /// </summary>
        public class NormalPolicyName
        {
            /// <summary>
            /// 管理者策略名
            /// </summary>
            /// <remarks>
            ///  Boss|admin|root
            /// </remarks>
            public const string ManagerPolicy = "Kdy_Manager_Policy";
        }

        /// <summary>
        /// 通用角色
        /// </summary>
        public class NormalRoleName
        {
            /// <summary>
            /// 超管
            /// </summary>
            public const string SuperAdmin = "root";
            public const string Boss = "boss";
            public const string Admin = "admin";
            public const string BossAndRoot = "root,boss";
        }

        public class ZcyClaimTypes
        {
            public const string UserNo = "ZcyUserNo";
            public const string CompanyId = "ZcyCompanyId";
        }

        public class EntityConst
        {
            /// <summary>
            /// 通用备注
            /// </summary>
            public const int BaseRemarkLength = 150;

            /// <summary>
            /// 通用名称
            /// </summary>
            public const int CommonName = 20;

            /// <summary>
            /// 通用用户名称
            /// </summary>
            public const int CommonPersonName = 10;
        }

    }
}
