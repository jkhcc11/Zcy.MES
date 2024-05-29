﻿using System.Collections.Generic;
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

        /// <summary>
        /// 根据工序Id获取产品工序
        /// </summary>
        /// <param name="productProcessesId">工序Id</param>
        /// <remarks>
        ///  会获取有效的产品工艺和产品
        /// </remarks>
        /// <returns></returns>
        Task<ProductProcess?> GetProductProcessesAsync(long productProcessesId);

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="productIds">产品Id集合</param>
        Task<List<Product>> GetValidProductListAsync(long[] productIds);
    }
}
