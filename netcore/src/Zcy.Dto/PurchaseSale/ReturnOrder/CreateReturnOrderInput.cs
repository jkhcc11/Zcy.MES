using System.ComponentModel.DataAnnotations;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 创建退货订单
    /// </summary>
    public class CreateReturnOrderInput : BaseCreateOrderInput
    {
        /// <summary>
        /// 供应商Id todo:对于散户订单 不友好，根据情况设置是否可选
        /// </summary>
        [Range(9999999, long.MaxValue, ErrorMessage = "供应商参数错误")]
        public long SupplierId { get; set; }

        /// <summary>
        /// 送货人
        /// </summary>
        /// <remarks>
        ///  应该是客户那边的人
        ///  ？？这个不确定是不是客户那边的用户
        /// </remarks>
        public string? ShipmentUser { get; set; }

        /// <summary>
        /// 退货订单详情
        /// </summary>
        public List<CreateReturnOrderDetailInput>? OrderDetails { get; set; }
    }

    /// <summary>
    /// 创建退货订单详情
    /// </summary>
    public class CreateReturnOrderDetailInput : BaseCreateOrderDetailInput
    {
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength, ErrorMessage = "订单产品备注参数错误")]
        public string? Remark { get; set; }
    }
}
