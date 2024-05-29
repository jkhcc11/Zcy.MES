using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.PurchaseSale;

namespace Zcy.IRepository.PurchaseSale
{
    /// <summary>
    /// 退货订单 仓储接口
    /// </summary>
    public interface IReturnOrderRepository : IBaseRepository<ReturnOrder, long>
    {
        /// <summary>
        /// 根据订单号获取退货订单
        /// </summary>
        /// <returns></returns>
        Task<ReturnOrder?> FirstOrDefaultAsync(string orderNo);
    }
}
