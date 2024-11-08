﻿using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Zcy.Entity.Company;
using Zcy.Entity.Products;
using Zcy.IRepository.Products;

namespace Zcy.MongoDB.Products
{
    /// <summary>
    /// 产品 仓储实现
    /// </summary>
    public class ProductRepository : BaseMongodbRepository<Product, long>, IProductRepository
    {
        private readonly IMongoCollection<ProductProcess> _productProcessCollection;
        private readonly IMongoCollection<ProductCraft> _productCraftCollection;

        public ProductRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {
            var productProcessDbName = $"{MongoDBConsts.DbPrefix}{nameof(ProductProcess)}";
            _productProcessCollection = ZcyMongodbContext.Database.GetCollection<ProductProcess>(productProcessDbName);

            var productCraftDbName = $"{MongoDBConsts.DbPrefix}{nameof(ProductCraft)}";
            _productCraftCollection = ZcyMongodbContext.Database.GetCollection<ProductCraft>(productCraftDbName);
        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <returns></returns>
        public override async Task<Product?> FirstOrDefaultAsync(long keyId)
        {
            var entity = await base.FirstOrDefaultAsync(keyId);
            if (entity is { ProductType: ProductTypeEnum.Processing })
            {
                entity.ProductProcesses = await _productProcessCollection
                    .Find(a => a.ProductId == entity.Id &&
                               a.CompanyId == entity.CompanyId &&
                               a.IsDelete == false)
                    .ToListAsync();
            }

            return entity;
        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <returns></returns>
        public override async Task<Product> GetEntityByIdAsync(long keyId)
        {
            var entity = await base.GetEntityByIdAsync(keyId);
            if (entity is { ProductType: ProductTypeEnum.Processing })
            {
                entity.ProductProcesses = await _productProcessCollection
                    .Find(a => a.ProductId == entity.Id &&
                               a.CompanyId == entity.CompanyId &&
                               a.IsDelete == false)
                    .ToListAsync();
            }

            return entity;
        }

        public override async Task<bool> CreateAsync(Product entity)
        {
            entity.CompanyId = LoginUserInfo?.CompanyId ?? 0;
            entity.CreatedUserId = LoginUserInfo?.UserId;
            entity.CreatedTime = DateTime.Now;

            var orderDetails = entity.ProductProcesses;

            await DbCollection.InsertOneAsync(entity);
            if (orderDetails != null && orderDetails.Any())
            {
                foreach (var item in orderDetails)
                {
                    item.CompanyId = LoginUserInfo?.CompanyId ?? 0;
                    item.CreatedUserId = LoginUserInfo?.UserId;
                    item.CreatedTime = DateTime.Now;
                }

                await _productProcessCollection.InsertManyAsync(orderDetails);
            }

            return true;

            //todo:看是否嵌入文档 单节点没开启事务
            //using var session = await DbCollection.Database.Client.StartSessionAsync();
            //session.StartTransaction();

            //try
            //{
            //    var orderDetails = entity.ProductProcesses;
            //    entity.ProductProcesses = null;

            //    await DbCollection.InsertOneAsync(session, entity);

            //    if (orderDetails != null && orderDetails.Any())
            //    {

            //        await _productProcessCollection.InsertManyAsync(session, orderDetails);
            //    }

            //    await session.CommitTransactionAsync();
            //    entity.ProductProcesses = orderDetails;
            //    return true;
            //}
            //catch (Exception)
            //{
            //    await session.AbortTransactionAsync();
            //    throw;
            //}
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public override Task<bool> CreateAsync(List<Product> entities)
        {
            throw new NotSupportedException("未实现批量创建");
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> DeleteAsync(Product entity)
        {
            if (entity.ProductProcesses != null &&
                entity.ProductProcesses.Any())
            {
                await DeleteProductProcessAsync(entity.ProductProcesses);
            }

            return await base.DeleteAsync(entity);
        }

        public override Task<bool> DeleteAsync(IEnumerable<Product> entities)
        {
            throw new NotSupportedException("未实现批量删除");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Product entity, ICollection<ProductProcess>? productProcesses)
        {
            //删除旧的
            if (productProcesses != null &&
                productProcesses.Any())
            {
                await DeleteProductProcessAsync(productProcesses);
            }

            if (entity.ProductProcesses != null &&
                entity.ProductProcesses.Any())
            {
                foreach (var item in entity.ProductProcesses)
                {
                    item.CompanyId = LoginUserInfo?.CompanyId ?? 0;
                    item.CreatedUserId = LoginUserInfo?.UserId;
                    item.CreatedTime = DateTime.Now;
                }

                //新增新的
                await _productProcessCollection.InsertManyAsync(entity.ProductProcesses);
            }

            return await base.UpdateAsync(entity);
        }

        /// <summary>
        /// 根据工序Id获取产品工序
        /// </summary>
        /// <param name="productProcessesId">工序Id</param>
        /// <remarks>
        ///  会获取有效的产品工艺和产品
        /// </remarks>
        /// <returns></returns>
        public async Task<ProductProcess?> GetProductProcessesAsync(long productProcessesId)
        {
            var productProcesses = await _productProcessCollection
                .Find(a => a.Id == productProcessesId &&
                           a.IsDelete == false)
                .FirstOrDefaultAsync();
            if (productProcesses == null)
            {
                return default;
            }

            productProcesses.Product = await DbCollection
                .Find(a => a.Id == productProcesses.ProductId &&
                           a.CompanyId == productProcesses.CompanyId &&
                           a.ProductStatus == PublicStatusEnum.Normal &&
                           a.IsDelete == false)
                .FirstOrDefaultAsync();

            productProcesses.ProductCraft = await _productCraftCollection
                .Find(a => a.Id == productProcesses.CraftId &&
                           a.CompanyId == productProcesses.CompanyId &&
                           a.CraftStatus == PublicStatusEnum.Normal &&
                           a.IsDelete == false)
                .FirstOrDefaultAsync();

            return productProcesses;
        }

        /// <summary>
        /// 根据工序Ids获取产品工序
        /// </summary>
        /// <param name="productProcessesIds">工序Ids</param>
        /// <remarks>
        ///  会获取有效的产品工艺和产品
        /// </remarks>
        /// <returns></returns>
        public async Task<List<ProductProcess>> GetProductProcessesAsync(long[] productProcessesIds)
        {
            var productProcesses = await _productProcessCollection
                .Find(a => productProcessesIds.Contains(a.Id) &&
                           a.IsDelete == false)
                .ToListAsync();
            if (productProcesses == null ||
                productProcesses.Any() == false)
            {
                return new List<ProductProcess>();
            }

            var companyId = productProcesses.First().CompanyId;
            //产品列表
            var productIds = productProcesses.Select(a => a.ProductId).ToArray();
            var products = await DbCollection
                .Find(a => productIds.Contains(a.Id) &&
                           a.CompanyId == companyId &&
                           a.ProductStatus == PublicStatusEnum.Normal &&
                           a.IsDelete == false)
                .ToListAsync();

            //工艺列表
            var productCraftIds = productProcesses.Select(a => a.CraftId).ToArray();
            var productCrafts = await _productCraftCollection
                .Find(a => productCraftIds.Contains(a.Id) &&
                           a.CompanyId == companyId &&
                           a.CraftStatus == PublicStatusEnum.Normal &&
                           a.IsDelete == false)
                .ToListAsync();

            foreach (var item in productProcesses)
            {
                item.Product = products.FirstOrDefault(a => a.Id == item.ProductId);
                item.ProductCraft = productCrafts.FirstOrDefault(a => a.Id == item.CraftId);
            }

            return productProcesses;
        }

        /// <summary>
        /// 获取有效产品列表
        /// </summary>
        /// <param name="productIds">产品Id集合</param>
        public async Task<List<Product>> GetValidProductListAsync(long[] productIds)
        {
            var query = await GetQueryableAsync();
            query = query.Where(a => productIds.Contains(a.Id) &&
                                     a.ProductStatus == PublicStatusEnum.Normal);
            return await (ToMongoQueryable(query)).ToListAsync();
        }

        /// <summary>
        /// 根据产品分类是否存在产品
        /// </summary>
        /// <param name="productTypeIds">产品分类ids</param>
        /// <returns></returns>
        public async Task<bool> ExistByProductTypeAsync(long[] productTypeIds)
        {
            var query = await GetQueryableAsync();
            query = query.Where(a => productTypeIds.Contains(a.ProductTypeId));
            return await (ToMongoQueryable(query)).AnyAsync();
        }

        /// <summary>
        /// 删除产品工序
        /// </summary>
        /// <returns></returns>
        private async Task DeleteProductProcessAsync(ICollection<ProductProcess> productProcesses)
        {
            // 定义批量操作列表
            var updates = new List<WriteModel<ProductProcess>>();
            foreach (var processItem in productProcesses)
            {
                // 添加更新操作
                var filter = Builders<ProductProcess>.Filter.Where(a => Equals(a.Id, processItem.Id));
                var update = Builders<ProductProcess>.Update
                    .Set(a => a.DeletedUserId, LoginUserInfo?.UserId)
                    .Set(a => a.DeletedTime, DateTime.Now)
                    .Set(a => a.IsDelete, true);
                updates.Add(new UpdateOneModel<ProductProcess>(filter, update));
            }

            await _productProcessCollection.BulkWriteAsync(updates);
        }
    }
}
