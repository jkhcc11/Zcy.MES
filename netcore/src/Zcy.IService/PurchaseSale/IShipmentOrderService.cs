using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.PurchaseSale;

namespace Zcy.IService.PurchaseSale
{
    /// <summary>
    /// 出货订单 服务接口
    /// </summary>
    public interface IShipmentOrderService : IZcyBaseService
    {
        /// <summary>
        /// 分页查询出货订单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageShipmentOrderDto>>> QueryPageShipmentOrderAsync(QueryPageShipmentOrderInput input);

        /// <summary>
        /// 创建出货订单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateShipmentOrderAsync(CreateShipmentOrderInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteAsync(long orderId);

        /// <summary>
        ///获取出货订单详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetShipmentOrderDetailDto>> GetShipmentOrderDetailAsync(long orderId);

        /// <summary>
        ///获取出货订单详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetShipmentOrderDetailDto>> GetShipmentOrderDetailAsync(string orderNo);
    }
}
