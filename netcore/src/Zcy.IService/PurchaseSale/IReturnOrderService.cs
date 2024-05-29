using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.PurchaseSale;

namespace Zcy.IService.PurchaseSale
{
    /// <summary>
    /// 退货订单 服务接口
    /// </summary>
    public interface IReturnOrderService : IZcyBaseService
    {
        /// <summary>
        /// 分页查询退货订单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageReturnOrderDto>>> QueryPageReturnOrderAsync(QueryPageReturnOrderInput input);

        /// <summary>
        /// 创建退货订单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateReturnOrderAsync(CreateReturnOrderInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteAsync(long orderId);

        /// <summary>
        ///获取退货订单详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetReturnOrderDetailDto>> GetReturnOrderDetailAsync(long orderId);

        /// <summary>
        ///获取退货订单详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetReturnOrderDetailDto>> GetReturnOrderDetailAsync(string orderNo);
    }
}
