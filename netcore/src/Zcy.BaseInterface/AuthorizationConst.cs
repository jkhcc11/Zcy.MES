namespace Zcy.BaseInterface
{

    /// <summary>
    /// 授权常量
    /// </summary>
    public class AuthorizationConst
    {
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
        }

        public class ZcyClaimTypes
        {
            public const string UserNo = "ZcyUserNo";
            public const string CompanyId = "ZcyCompanyId";
        }
    }
}
