using Zcy.Entity.PurchaseSale;
using Zcy.IRepository.PurchaseSale;

namespace Zcy.MongoDB.PurchaseSale
{
    /// <summary>
    /// 销售订单详情 仓储实现
    /// </summary>
    public class SaleOrderDetailRepository : BaseMongodbRepository<SaleOrderDetail, long>, ISaleOrderDetailRepository
    {
        public SaleOrderDetailRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {

        }
    }
}
