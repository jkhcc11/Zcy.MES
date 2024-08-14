using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.PurchaseSale;
using Zcy.Entity.PurchaseSale.TotalsModels;

namespace Zcy.IRepository.PurchaseSale
{
    /// <summary>
    /// 出货订单 仓储接口
    /// </summary>
    public interface IShipmentOrderRepository : IBaseRepository<ShipmentOrder, long>
    {
        /// <summary>
        /// 根据订单号获取出货订单
        /// </summary>
        /// <returns></returns>
        Task<ShipmentOrder?> FirstOrDefaultAsync(string orderNo);

        /// <summary>
        /// 采购订单统计(按天)
        /// </summary>
        /// <returns></returns>
        Task<List<ShipmentOrderTotalsTemp>> PurchaseOrderTotalsAsync(IQueryable<ShipmentOrder> queryable);
    }
}
