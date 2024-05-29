using MongoDB.Driver;
using Zcy.Entity.PurchaseSale;
using Zcy.IRepository.PurchaseSale;

namespace Zcy.MongoDB.PurchaseSale
{
    /// <summary>
    /// 退货订单 仓储实现
    /// </summary>
    public class ReturnOrderRepository : BaseMongodbRepository<ReturnOrder, long>, IReturnOrderRepository
    {
        private readonly IMongoCollection<ReturnOrderDetail> _orderDetailCollection;
        public ReturnOrderRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {
            var detailDbName = $"{MongoDBConsts.DbPrefix}{nameof(ReturnOrderDetail)}";
            _orderDetailCollection = ZcyMongodbContext.Database.GetCollection<ReturnOrderDetail>(detailDbName);

        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <returns></returns>
        public override async Task<ReturnOrder> GetEntityByIdAsync(long keyId)
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
        public override async Task<ReturnOrder?> FirstOrDefaultAsync(long keyId)
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
        public override async Task<bool> CreateAsync(ReturnOrder entity)
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
        public override Task<bool> CreateAsync(List<ReturnOrder> entities)
        {
            throw new NotSupportedException("未实现批量创建");
        }


        /// <summary>
        /// 软删除
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> DeleteAsync(ReturnOrder entity)
        {
            if (entity.OrderDetails != null &&
                entity.OrderDetails.Any())
            {
                await DeleteProductProcessAsync(entity.OrderDetails);
            }

            return await base.DeleteAsync(entity);
        }

        public override Task<bool> DeleteAsync(IEnumerable<ReturnOrder> entities)
        {
            throw new NotSupportedException("未实现批量删除");
        }

        /// <summary>
        /// 根据订单号获取退货订单
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnOrder?> FirstOrDefaultAsync(string orderNo)
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
        /// 删除详情
        /// </summary>
        /// <returns></returns>
        private async Task DeleteProductProcessAsync(ICollection<ReturnOrderDetail> productProcesses)
        {
            // 定义批量操作列表
            var updates = new List<WriteModel<ReturnOrderDetail>>();
            foreach (var processItem in productProcesses)
            {
                // 添加更新操作
                var filter = Builders<ReturnOrderDetail>.Filter.Where(a => Equals(a.Id, processItem.Id));
                var update = Builders<ReturnOrderDetail>.Update
                    .Set(a => a.DeletedUserId, LoginUserInfo?.UserId)
                    .Set(a => a.DeletedTime, DateTime.Now)
                    .Set(a => a.IsDelete, true);
                updates.Add(new UpdateOneModel<ReturnOrderDetail>(filter, update));
            }

            await _orderDetailCollection.BulkWriteAsync(updates);
        }
    }
}
