using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.PurchaseSale;

namespace Zcy.IService.PurchaseSale
{
    /// <summary>
    /// 销售订单 服务接口
    /// </summary>
    public interface ISaleOrderService : IZcyBaseService
    {
        /// <summary>
        /// 分页查询销售订单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageSaleOrderDto>>> QueryPageSaleOrderAsync(QueryPageSaleOrderInput input);

        /// <summary>
        /// 创建销售订单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateSaleOrderAsync(CreateSaleOrderInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteAsync(long orderId);

        /// <summary>
        ///获取销售订单详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetSaleOrderDetailDto>> GetSaleOrderDetailAsync(long orderId);

        /// <summary>
        ///获取销售订单详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetSaleOrderDetailDto>> GetSaleOrderDetailAsync(string orderNo);

        /// <summary>
        /// 获取销售订单汇总
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetSaleOrderTotalsDto>> GetSaleOrderTotalsAsync(QueryPageSaleOrderInput input);
    }
}
