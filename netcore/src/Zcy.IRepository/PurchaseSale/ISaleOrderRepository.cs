using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.PurchaseSale;

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
    }
}
