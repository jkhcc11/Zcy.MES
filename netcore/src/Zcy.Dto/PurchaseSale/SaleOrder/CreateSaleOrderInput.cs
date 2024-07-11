using System.ComponentModel.DataAnnotations;
using Zcy.Entity.FinancialMemo;
using Zcy.Entity.PurchaseSale;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 创建销售订单
    /// </summary>
    public class CreateSaleOrderInput : BaseCreateOrderInput
    {
        /// <summary>
        /// 账号类型
        /// </summary>
        [EnumDataType(typeof(AccountTypeEnum))]
        public AccountTypeEnum? AccountType { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        [Range(9999999, long.MaxValue, ErrorMessage = "供应商参数错误")]
        public long SupplierId { get; set; }

        /// <summary>
        /// 运费价格
        /// </summary>
        [Range(0, 9999999)]
        public decimal FreightPrice { get; set; }

        /// <summary>
        /// 销售单状态
        /// </summary>
        [EnumDataType(typeof(SaleOrderStatusEnum))]
        public SaleOrderStatusEnum? SaleOrderStatus { get; set; }

        /// <summary>
        /// 销售订单详情
        /// </summary>
        public List<CreateSaleOrderDetailInput>? OrderDetails { get; set; }
    }

    /// <summary>
    /// 创建销售订单详情
    /// </summary>
    public class CreateSaleOrderDetailInput : BaseCreateOrderDetailInput
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
