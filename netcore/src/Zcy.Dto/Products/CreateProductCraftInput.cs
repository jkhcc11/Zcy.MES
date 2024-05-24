using System.ComponentModel.DataAnnotations;
using Zcy.Entity.Company;
using Zcy.Entity.Products;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 创建产品工艺
    /// </summary>
    public class CreateProductCraftInput
    {
        public CreateProductCraftInput(string craftName)
        {
            CraftName = craftName;
        }

        /// <summary>
        /// 工艺名
        /// </summary>
        [Required(ErrorMessage = "工艺名不能为空")]
        public string CraftName { get; set; }

        /// <summary>
        /// 工艺类型
        /// </summary>
        /// <remarks>
        /// 独立工艺: ■一般为计时 不用配置产品线使用。比如捡货计时|收货计时 <br/>
        /// 加工工艺：■配合产品使用
        /// </remarks>
        [EnumDataType(typeof(CraftTypeEnum))]
        public CraftTypeEnum CraftType { get; set; }

        /// <summary>
        /// 计费类型
        /// </summary>
        [EnumDataType(typeof(BillingTypeEnum))]
        public BillingTypeEnum BillingType { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        /// <remarks>
        /// 工序基础价格，如果有根据产品+工序的在产品选择工序后加
        /// </remarks>
        [Range(0.01, 99999999)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength)]
        public string? Remark { get; set; }
    }
}
