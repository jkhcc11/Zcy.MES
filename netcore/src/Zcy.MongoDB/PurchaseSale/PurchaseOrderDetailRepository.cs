using MongoDB.Driver;
using Zcy.Entity.PurchaseSale;
using Zcy.IRepository.PurchaseSale;

namespace Zcy.MongoDB.PurchaseSale
{
    /// <summary>
    /// 采购订单详情 仓储实现
    /// </summary>
    public class PurchaseOrderDetailRepository : BaseMongodbRepository<PurchaseOrderDetail, long>, IPurchaseOrderDetailRepository
    {
        public PurchaseOrderDetailRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {

        }

        /// <summary>
        /// 根据产品Id获取采购详情
        /// </summary>
        /// <returns></returns>
        public async Task<List<PurchaseOrderDetail>> GetPurchaseOrderDetailByProductIdsAsync(long[] productIds)
        {
            var query = await GetQueryableAsync();
            query = query.Where(a => productIds.Contains(a.ProductId));
            return await ToMongoQueryable(query).ToListAsync();
        }
    }
}
