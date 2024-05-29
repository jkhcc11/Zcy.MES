using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.Dto.PurchaseSale;
using Zcy.IService.PurchaseSale;
using Microsoft.AspNetCore.Authorization;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 销售订单
    /// </summary>
    public class SaleOrderController : BaseManagerController
    {
        private readonly ISaleOrderService _saleOrderService;

        public SaleOrderController(ISaleOrderService saleOrderService)
        {
            _saleOrderService = saleOrderService;
        }


        /// <summary>
        /// 创建销售订单
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<KdyResult> CreateSaleOrderAsync(CreateSaleOrderInput input)
        {
            var result = await _saleOrderService.CreateSaleOrderAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询销售订单
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.Boss)]
        public async Task<KdyResult<QueryPageDto<QueryPageSaleOrderDto>>> QueryPageSaleOrderAsync(
            [FromQuery] QueryPageSaleOrderInput input)
        {
            var result = await _saleOrderService.QueryPageSaleOrderAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.Boss)]
        public async Task<KdyResult> DeleteAsync(long id)
        {
            var result = await _saleOrderService.DeleteAsync(id);
            return result;
        }

        /// <summary>
        /// 获取销售订单
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-detail/{id}")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.Boss)]
        public async Task<KdyResult<GetSaleOrderDetailDto>> GetSaleOrderDetailAsync(long id)
        {
            var result = await _saleOrderService.GetSaleOrderDetailAsync(id);
            return result;
        }
    }
}
