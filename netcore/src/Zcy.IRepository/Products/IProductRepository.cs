using System.Collections.Generic;
using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.Products;

namespace Zcy.IRepository.Products
{
    /// <summary>
    /// 产品 仓储接口
    /// </summary>
    public interface IProductRepository : IBaseRepository<Product, long>
    {
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateAsync(Product entity, ICollection<ProductProcess>? productProcesses);
    }
}
