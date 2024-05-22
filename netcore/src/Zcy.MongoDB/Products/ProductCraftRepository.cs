using Zcy.Entity.Products;
using Zcy.IRepository.Products;

namespace Zcy.MongoDB.Products
{
    /// <summary>
    ///  产品工艺 仓储实现
    /// </summary>
    public class ProductCraftRepository : BaseMongodbRepository<ProductCraft, long>, IProductCraftRepository
    {
        public ProductCraftRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {

        }
    }
}
