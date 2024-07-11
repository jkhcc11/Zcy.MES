

using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Zcy.Dto.User
{
    /// <summary>
    /// 小程序登录信息
    /// </summary>
    public class GetLoginInfoWithWxDto
    {
        /// <summary>
        /// 基础数据
        /// </summary>
        public bool IsShowBaseData { get; set; }
        
        /// <summary>
        /// 采购销售
        /// </summary>
        public bool IsPurchaseSaleShow { get; set; }
        
        /// <summary>
        /// 财务备忘录
        /// </summary>
        public bool IsFinancialMemoShow { get; set; }
        
        /// <summary>
        /// 我的报工
        /// </summary>
        public bool MeReport{ get; set; }

        /// <summary>
        /// 管理报工
        /// </summary>
        public bool AdminReport { get; set; }

        /// <summary>
        /// 报工统计
        /// </summary>
        public bool BossReport { get; set; }

        /// <summary>
        /// 是否禁用状态
        /// </summary>
        /// <remarks>
        /// 禁用状态时，清空登录信息 退出
        /// </remarks>
        public bool IsBan { get; set; }

        public void SetAdmin()
        {
            IsShowBaseData = true;
            AdminReport = true;
            MeReport= true;
        }

        public void SetBoss()
        {
            IsShowBaseData = true;
            IsPurchaseSaleShow = true;
            IsFinancialMemoShow = true;

            AdminReport = true;
            MeReport = true;
            BossReport = true;
        }
    }
}
