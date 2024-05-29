using MongoDB.Driver;
using Zcy.Entity.PurchaseSale;
using Zcy.IRepository.PurchaseSale;

namespace Zcy.MongoDB.PurchaseSale
{
    /// <summary>
    /// 采购订单 仓储实现
    /// </summary>
    public class PurchaseOrderRepository : BaseMongodbRepository<PurchaseOrder, long>, IPurchaseOrderRepository
    {
        private readonly IMongoCollection<PurchaseOrderDetail> _purchaseOrderCollection;
        public PurchaseOrderRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {
            var detailDbName = $"{MongoDBConsts.DbPrefix}{nameof(PurchaseOrderDetail)}";
            _purchaseOrderCollection = ZcyMongodbContext.Database.GetCollection<PurchaseOrderDetail>(detailDbName);

        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <returns></returns>
        public override async Task<PurchaseOrder> GetEntityByIdAsync(long keyId)
        {
            var entity = await base.GetEntityByIdAsync(keyId);
            entity.OrderDetails = await _purchaseOrderCollection
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
        public override async Task<PurchaseOrder?> FirstOrDefaultAsync(long keyId)
        {
            var entity = await base.FirstOrDefaultAsync(keyId);
            if (entity == null)
            {
                return default;
            }

            entity.OrderDetails = await _purchaseOrderCollection
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
        public override async Task<bool> CreateAsync(PurchaseOrder entity)
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

                await _purchaseOrderCollection.InsertManyAsync(entity.OrderDetails);
            }

            return true;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public override Task<bool> CreateAsync(List<PurchaseOrder> entities)
        {
            throw new NotSupportedException("未实现批量创建");
        }


        /// <summary>
        /// 软删除
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> DeleteAsync(PurchaseOrder entity)
        {
            if (entity.OrderDetails != null &&
                entity.OrderDetails.Any())
            {
                await DeleteProductProcessAsync(entity.OrderDetails);
            }

            return await base.DeleteAsync(entity);
        }

        public override Task<bool> DeleteAsync(IEnumerable<PurchaseOrder> entities)
        {
            throw new NotSupportedException("未实现批量删除");
        }

        /// <summary>
        /// 根据订单号获取采购订单
        /// </summary>
        /// <returns></returns>
        public async Task<PurchaseOrder?> FirstOrDefaultAsync(string orderNo)
        {
            var entity = await base.FirstOrDefaultAsync(a => a.OrderNo == orderNo);
            if (entity == null)
            {
                return default;
            }

            entity.OrderDetails = await _purchaseOrderCollection
                .Find(a => a.OrderId == entity.Id &&
                           a.CompanyId == entity.CompanyId &&
                           a.IsDelete == false)
                .ToListAsync();
            return entity;
        }

        /// <summary>
        /// 删除详情
        /// </summary>
        /// <returns></returns>
        private async Task DeleteProductProcessAsync(ICollection<PurchaseOrderDetail> productProcesses)
        {
            // 定义批量操作列表
            var updates = new List<WriteModel<PurchaseOrderDetail>>();
            foreach (var processItem in productProcesses)
            {
                // 添加更新操作
                var filter = Builders<PurchaseOrderDetail>.Filter.Where(a => Equals(a.Id, processItem.Id));
                var update = Builders<PurchaseOrderDetail>.Update
                    .Set(a => a.DeletedUserId, LoginUserInfo?.UserId)
                    .Set(a => a.DeletedTime, DateTime.Now)
                    .Set(a => a.IsDelete, true);
                updates.Add(new UpdateOneModel<PurchaseOrderDetail>(filter, update));
            }

            await _purchaseOrderCollection.BulkWriteAsync(updates);
        }
    }
}
