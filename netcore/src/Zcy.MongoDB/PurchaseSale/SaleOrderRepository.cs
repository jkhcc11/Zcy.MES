using System.Linq.Dynamic.Core;
using MongoDB.Driver;
using Zcy.Entity.PurchaseSale;
using Zcy.Entity.PurchaseSale.TotalsModels;
using Zcy.IRepository.PurchaseSale;

namespace Zcy.MongoDB.PurchaseSale
{
    /// <summary>
    /// 销售订单 仓储实现
    /// </summary>
    public class SaleOrderRepository : BaseMongodbRepository<SaleOrder, long>, ISaleOrderRepository
    {
        private readonly IMongoCollection<SaleOrderDetail> _orderDetailCollection;
        public SaleOrderRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {
            var detailDbName = $"{MongoDBConsts.DbPrefix}{nameof(SaleOrderDetail)}";
            _orderDetailCollection = ZcyMongodbContext.Database.GetCollection<SaleOrderDetail>(detailDbName);

        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <returns></returns>
        public override async Task<SaleOrder> GetEntityByIdAsync(long keyId)
        {
            var entity = await base.GetEntityByIdAsync(keyId);
            entity.OrderDetails = await _orderDetailCollection
                .Find(a => a.OrderId == entity.Id &&
                           a.CompanyId == entity.CompanyId &&
                           a.IsDelete == false)
                .ToListAsync();
            return entity;
        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <returns></returns>
        public override async Task<SaleOrder?> FirstOrDefaultAsync(long keyId)
        {
            var entity = await base.FirstOrDefaultAsync(keyId);
            if (entity == null)
            {
                return default;
            }

            entity.OrderDetails = await _orderDetailCollection
                .Find(a => a.OrderId == entity.Id &&
                           a.CompanyId == entity.CompanyId &&
                           a.IsDelete == false)
                .ToListAsync();
            return entity;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> CreateAsync(SaleOrder entity)
        {
            entity.CompanyId = LoginUserInfo?.CompanyId ?? 0;
            entity.CreatedUserId = LoginUserInfo?.UserId;
            entity.CreatedTime = DateTime.Now;

            await DbCollection.InsertOneAsync(entity);
            if (entity.OrderDetails != null && entity.OrderDetails.Any())
            {
                foreach (var item in entity.OrderDetails)
                {
                    item.CompanyId = LoginUserInfo?.CompanyId ?? 0;
                    item.CreatedUserId = LoginUserInfo?.UserId;
                    item.CreatedTime = DateTime.Now;
                }

                await _orderDetailCollection.InsertManyAsync(entity.OrderDetails);
            }

            return true;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public override Task<bool> CreateAsync(List<SaleOrder> entities)
        {
            throw new NotSupportedException("未实现批量创建");
        }


        /// <summary>
        /// 软删除
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> DeleteAsync(SaleOrder entity)
        {
            if (entity.OrderDetails != null &&
                entity.OrderDetails.Any())
            {
                await DeleteProductProcessAsync(entity.OrderDetails);
            }

            return await base.DeleteAsync(entity);
        }

        public override Task<bool> DeleteAsync(IEnumerable<SaleOrder> entities)
        {
            throw new NotSupportedException("未实现批量删除");
        }

        /// <summary>
        /// 根据订单号获取销售订单
        /// </summary>
        /// <returns></returns>
        public async Task<SaleOrder?> FirstOrDefaultAsync(string orderNo)
        {
            var entity = await base.FirstOrDefaultAsync(a => a.OrderNo == orderNo);
            if (entity == null)
            {
                return default;
            }

            entity.OrderDetails = await _orderDetailCollection
                .Find(a => a.OrderId == entity.Id &&
                           a.CompanyId == entity.CompanyId &&
                           a.IsDelete == false)
                .ToListAsync();
            return entity;
        }

        /// <summary>
        /// 销售订单统计(按天)
        /// </summary>
        /// <returns></returns>
        public async Task<List<SaleOrderTotalsTemp>> SaleOrderTotalsAsync(IQueryable<SaleOrder> queryable)
        {
            var tempList = await ToMongoQueryable(queryable)
                .GroupBy(a => a.OrderDate)
                .Select(a => new SaleOrderTotalsTemp()
                {
                    TotalsDate = a.Key,
                    OrderPrice = a.Sum(b => b.OrderPrice),
                    FreightPrice = a.Sum(b => b.FreightPrice),
                    SumSalePrice = a.Sum(b => b.SumSalePrice),
                })
                .ToDynamicListAsync<SaleOrderTotalsTemp>();

            return tempList;
        }

        /// <summary>
        /// 删除详情
        /// </summary>
        /// <returns></returns>
        private async Task DeleteProductProcessAsync(ICollection<SaleOrderDetail> productProcesses)
        {
            // 定义批量操作列表
            var updates = new List<WriteModel<SaleOrderDetail>>();
            foreach (var processItem in productProcesses)
            {
                // 添加更新操作
                var filter = Builders<SaleOrderDetail>.Filter.Where(a => Equals(a.Id, processItem.Id));
                var update = Builders<SaleOrderDetail>.Update
                    .Set(a => a.DeletedUserId, LoginUserInfo?.UserId)
                    .Set(a => a.DeletedTime, DateTime.Now)
                    .Set(a => a.IsDelete, true);
                updates.Add(new UpdateOneModel<SaleOrderDetail>(filter, update));
            }

            await _orderDetailCollection.BulkWriteAsync(updates);
        }
    }
}
