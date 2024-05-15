using Zcy.BaseInterface.Entities;
using Zcy.Entity.Company;

namespace Zcy.Entity.Products
{
    /// <summary>
    /// 产品工艺
    /// </summary>
    public class ProductCraft : BaseEntity<long>, IBaseCompany
    {
        /// <summary>
        /// 工艺
        /// </summary>
        /// <param name="craftName">工艺名</param>
        /// <param name="craftType">工艺类型</param>
        /// <param name="billingType">计费类型</param>
        /// <param name="unitPrice">单价</param>
        public ProductCraft(string craftName, CraftTypeEnum craftType,
            BillingTypeEnum billingType, decimal unitPrice)
        {
            CraftName = craftName;
            CraftType = craftType;
            BillingType = billingType;
            UnitPrice = unitPrice;
            CraftStatus = PublicStatusEnum.Normal;
        }

        /// <summary>
        /// 工艺名
        /// </summary>
        public string CraftName { get; set; }

        /// <summary>
        /// 工艺类型
        /// </summary>
        /// <remarks>
        /// 独立工艺:■一般为计时 不用配置产品线使用。比如捡货计时|收货计时 <br/>
        /// 加工工艺：■配合产品使用
        /// </remarks>
        public CraftTypeEnum CraftType { get; set; }

        /// <summary>
        /// 计费类型
        /// </summary>
        public BillingTypeEnum BillingType { get; set; }

        /// <summary>
        /// 工艺状态
        /// </summary>
        public PublicStatusEnum CraftStatus { get; protected set; }

        /// <summary>
        /// 单价
        /// </summary>
        /// <remarks>
        /// 工序基础价格，如果有根据产品+工序的在产品选择工序后加
        /// </remarks>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public void Ban()
        {
            CraftStatus = PublicStatusEnum.Ban;
        }
    }
}
