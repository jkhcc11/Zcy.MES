using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.PurchaseSale;
using Zcy.Entity.PurchaseSale.TotalsModels;

namespace Zcy.IRepository.PurchaseSale
{
    /// <summary>
    /// 采购订单 仓储接口
    /// </summary>
    public interface IPurchaseOrderRepository : IBaseRepository<PurchaseOrder, long>
    {
        /// <summary>
        /// 根据订单号获取采购订单
        /// </summary>
        /// <returns></returns>
        Task<PurchaseOrder?> FirstOrDefaultAsync(string orderNo);

        /// <summary>
        /// 采购订单统计(按天)
        /// </summary>
        /// <returns></returns>
        Task<List<PurchaseOrderTotalsTemp>> PurchaseOrderTotalsAsync(IQueryable<PurchaseOrder> queryable);
    }
}
