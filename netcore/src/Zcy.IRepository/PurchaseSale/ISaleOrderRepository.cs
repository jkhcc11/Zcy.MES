using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.PurchaseSale;
using Zcy.Entity.PurchaseSale.TotalsModels;

namespace Zcy.IRepository.PurchaseSale
{
    /// <summary>
    /// 销售订单 仓储接口
    /// </summary>
    public interface ISaleOrderRepository : IBaseRepository<SaleOrder, long>
    {
        /// <summary>
        /// 根据订单号获取销售订单
        /// </summary>
        /// <returns></returns>
        Task<SaleOrder?> FirstOrDefaultAsync(string orderNo);

        /// <summary>
        /// 销售订单统计(按天)
        /// </summary>
        /// <returns></returns>
        Task<List<SaleOrderTotalsTemp>> SaleOrderTotalsAsync(IQueryable<SaleOrder> queryable);
    }
}
