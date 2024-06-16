using System.Collections.Generic;
using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.PurchaseSale;

namespace Zcy.IRepository.PurchaseSale
{
    /// <summary>
    /// 采购订单详情 仓储接口
    /// </summary>
    public interface IPurchaseOrderDetailRepository : IBaseRepository<PurchaseOrderDetail, long>
    {
        /// <summary>
        /// 根据产品Id获取采购详情
        /// </summary>
        /// <returns></returns>
        Task<List<PurchaseOrderDetail>> GetPurchaseOrderDetailByProductIdsAsync(long[] productIds);
    }
}
