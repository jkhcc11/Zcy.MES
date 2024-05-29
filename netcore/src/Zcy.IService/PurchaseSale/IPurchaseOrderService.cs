using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.PurchaseSale;

namespace Zcy.IService.PurchaseSale
{
    /// <summary>
    /// 采购订单 服务接口
    /// </summary>
    public interface IPurchaseOrderService : IZcyBaseService
    {
        /// <summary>
        /// 分页查询采购订单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPagePurchaseOrderDto>>> QueryPagePurchaseOrderAsync(QueryPagePurchaseOrderInput input);

        /// <summary>
        /// 创建采购订单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreatePurchaseOrderAsync(CreatePurchaseOrderInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteAsync(long orderId);

        /// <summary>
        ///获取采购订单详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetPurchaseOrderDetailDto>> GetPurchaseOrderDetailAsync(long orderId);

        /// <summary>
        ///获取采购订单详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetPurchaseOrderDetailDto>> GetPurchaseOrderDetailAsync(string orderNo);
    }
}
