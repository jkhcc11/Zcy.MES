using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.Dto.PurchaseSale;
using Zcy.IService.PurchaseSale;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 出货订单
    /// </summary>
    public class ShipmentOrderController : BaseManagerController
    {
        private readonly IShipmentOrderService _shipmentOrderService;

        public ShipmentOrderController(IShipmentOrderService shipmentOrderService)
        {
            _shipmentOrderService = shipmentOrderService;
        }


        /// <summary>
        /// 创建出货订单
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<KdyResult> CreateShipmentOrderAsync(CreateShipmentOrderInput input)
        {
            var result = await _shipmentOrderService.CreateShipmentOrderAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询出货订单
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageShipmentOrderDto>>> QueryPageShipmentOrderAsync(
            [FromQuery] QueryPageShipmentOrderInput input)
        {
            var result = await _shipmentOrderService.QueryPageShipmentOrderAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<KdyResult> DeleteAsync(long id)
        {
            var result = await _shipmentOrderService.DeleteAsync(id);
            return result;
        }

        /// <summary>
        /// 获取出货订单
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-detail/{id}")]
        public async Task<KdyResult<GetShipmentOrderDetailDto>> GetShipmentOrderDetailAsync(long id)
        {
            var result = await _shipmentOrderService.GetShipmentOrderDetailAsync(id);
            return result;
        }
    }
}
