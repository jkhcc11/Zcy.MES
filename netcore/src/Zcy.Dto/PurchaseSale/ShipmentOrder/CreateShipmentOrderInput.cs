using System.ComponentModel.DataAnnotations;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.PurchaseSale
{
    /// <summary>
    /// 创建出货订单
    /// </summary>
    public class CreateShipmentOrderInput : BaseCreateOrderInput
    {
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Range(9999999, long.MaxValue, ErrorMessage = "供应商参数错误")]
        public long SupplierId { get; set; }

        /// <summary>
        /// 提货人
        /// </summary>
        /// <remarks>
        ///  应该是客户那边的人
        ///  ？？这个不确定是不是客户那边的用户
        /// </remarks>
        public string? PickUpUser { get; set; }

        /// <summary>
        /// 出货订单详情
        /// </summary>
        public List<CreateShipmentOrderDetailInput>? OrderDetails { get; set; }
    }

    /// <summary>
    /// 创建出货订单详情
    /// </summary>
    public class CreateShipmentOrderDetailInput : BaseCreateOrderDetailInput
    {
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength, ErrorMessage = "订单产品备注参数错误")]
        public string? Remark { get; set; }
    }
}
