using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.Dto.PurchaseSale;
using Zcy.IService.PurchaseSale;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 退货订单
    /// </summary>
    public class ReturnOrderController : BaseManagerController
    {
        private readonly IReturnOrderService _returnOrderService;

        public ReturnOrderController(IReturnOrderService returnOrderService)
        {
            _returnOrderService = returnOrderService;
        }


        /// <summary>
        /// 创建退货订单
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<KdyResult> CreateReturnOrderAsync(CreateReturnOrderInput input)
        {
            var result = await _returnOrderService.CreateReturnOrderAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询退货订单
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageReturnOrderDto>>> QueryPageReturnOrderAsync(
            [FromQuery] QueryPageReturnOrderInput input)
        {
            var result = await _returnOrderService.QueryPageReturnOrderAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<KdyResult> DeleteAsync(long id)
        {
            var result = await _returnOrderService.DeleteAsync(id);
            return result;
        }

        /// <summary>
        /// 获取退货订单
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-detail/{id}")]
        public async Task<KdyResult<GetReturnOrderDetailDto>> GetShipmentOrderDetailAsync(long id)
        {
            var result = await _returnOrderService.GetReturnOrderDetailAsync(id);
            return result;
        }

        /// <summary>
        /// 获取退货订单
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-detail-byNo/{orderNo}")]
        public async Task<KdyResult<GetReturnOrderDetailDto>> GetShipmentOrderDetailAsync(string orderNo)
        {
            var result = await _returnOrderService.GetReturnOrderDetailAsync(orderNo);
            return result;
        }
    }
}
