using System.ComponentModel.DataAnnotations;
using Zcy.Entity.FinancialMemo;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 创建采购订单
    /// </summary>
    public class CreatePurchaseOrderInput : BaseCreateOrderInput
    {
        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountTypeEnum? AccountType { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        [Range(9999999, long.MaxValue, ErrorMessage = "供应商参数错误")]
        public long SupplierId { get; set; }

        /// <summary>
        /// 采购订单详情
        /// </summary>
        public List<CreatePurchaseOrderDetailInput>? OrderDetails { get; set; }
    }

    /// <summary>
    /// 创建采购订单详情
    /// </summary>
    public class CreatePurchaseOrderDetailInput : BaseCreateOrderDetailInput
    {
        /// <summary>
        /// 单价
        /// </summary>
        [Range(0.01, 9999999)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength, ErrorMessage = "订单产品备注参数错误")]
        public string? Remark { get; set; }
    }
}
