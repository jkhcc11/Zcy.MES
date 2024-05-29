using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.Dto.PurchaseSale;
using Zcy.IService.PurchaseSale;
using Microsoft.AspNetCore.Authorization;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 采购订单 todo:待确认权限
    /// </summary>
    [Authorize(Roles = AuthorizationConst.NormalRoleName.Boss)]
    public class PurchaseOrderController : BaseManagerController
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        /// <summary>
        /// 创建采购订单
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<KdyResult> CreatePurchaseOrderAsync(CreatePurchaseOrderInput input)
        {
            var result = await _purchaseOrderService.CreatePurchaseOrderAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询采购订单
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPagePurchaseOrderDto>>> QueryPagePurchaseOrderAsync(
            [FromQuery] QueryPagePurchaseOrderInput input)
        {
            var result = await _purchaseOrderService.QueryPagePurchaseOrderAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<KdyResult> DeleteAsync(long id)
        {
            var result = await _purchaseOrderService.DeleteAsync(id);
            return result;
        }

        /// <summary>
        /// 获取采购订单详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-detail/{id}")]
        public async Task<KdyResult<GetPurchaseOrderDetailDto>> GetPurchaseOrderDetailAsync(long id)
        {
            var result = await _purchaseOrderService.GetPurchaseOrderDetailAsync(id);
            return result;
        }
    }
}
