using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Entities;
using Zcy.BaseInterface.Service;

namespace Zcy.MongoDB
{
    /// <summary>
    /// Mongodb通用仓储 实现
    /// </summary>
    /// <typeparam name="TEntity">实体类 继承<see cref="BaseEntity{TKey}"/></typeparam>
    /// <typeparam name="TKey">主键</typeparam>
    public abstract class BaseMongodbRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey> where TKey : struct
    {
        protected readonly IdGenerateExtension IdGenerateExtension;
        protected readonly ZcyMongodbContext ZcyMongodbContext;
        protected readonly string CollectionName;
        protected readonly IMongoCollection<TEntity> DbCollection;
        protected readonly IServiceProvider ServiceProvider;
        protected readonly ILoginUserInfo? LoginUserInfo;
        protected readonly IMapper BaseMapper;

        protected BaseMongodbRepository(ZcyMongodbContext zcyDbContext)
        {
            ZcyMongodbContext = zcyDbContext;
            CollectionName = $"{MongoDBConsts.DbPrefix}{typeof(TEntity).Name}";
            DbCollection = zcyDbContext.Database.GetCollection<TEntity>(CollectionName);
            ServiceProvider = zcyDbContext.ServiceCollection.BuildServiceProvider();
            LoginUserInfo = ServiceProvider.GetService<ILoginUserInfo>();
            IdGenerateExtension = ServiceProvider.GetService<IdGenerateExtension>() ??
                                  throw new NullReferenceException("BaseMongodbRepository IdGenerateExtension is null");
            BaseMapper = ServiceProvider.GetService<IMapper>() ??
                         throw new NullReferenceException("BaseMongodbRepository BaseMapper is null");
        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TEntity?> FirstOrDefaultAsync(TKey keyId)
        {
            var query = await GetQueryableAsync();
            return await ToMongoQueryable(query.Where(a => Equals(a.Id, keyId)))
                .FirstOrDefaultAsync();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = await GetQueryableAsync();
            return await ToMongoQueryable(query.Where(predicate))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TEntity> GetEntityByIdAsync(TKey keyId)
        {
            var query = await GetQueryableAsync();
            return await ToMongoQueryable(query.Where(a => Equals(a.Id, keyId)))
               .FirstAsync();
        }

        public virtual async Task<bool> CreateAsync(TEntity entity)
        {
            // 检查 TKey 是否为 long 类型 并且如果默认值才赋值
            //有些情况，需要Id关联的那种 是提前entity赋值，不覆盖
            if (typeof(TKey) == typeof(long) &&
                 EqualityComparer<TKey>.Default.Equals(entity.Id, default))
            {
                entity.Id = (TKey)(object)IdGenerateExtension.GenerateId();
            }

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (entity is IBaseCompany companyEntity)
            {
                companyEntity.CompanyId = LoginUserInfo?.CompanyId ?? 0;
            }

            entity.CreatedUserId = LoginUserInfo?.UserId;
            entity.CreatedTime = DateTime.Now;
            await DbCollection.InsertOneAsync(entity);
            return true;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> CreateAsync(List<TEntity> entities)
        {
            foreach (var item in entities)
            {
                item.CreatedUserId = LoginUserInfo?.UserId;
                item.CreatedTime = DateTime.Now;

                // ReSharper disable once SuspiciousTypeConversion.Global
                if (item is IBaseCompany companyEntity)
                {
                    companyEntity.CompanyId = LoginUserInfo?.CompanyId ?? 0;
                }

                // 检查 TKey 是否为 long 类型
                if (typeof(TKey) == typeof(long) &&
                    EqualityComparer<TKey>.Default.Equals(item.Id, default))
                {
                    item.Id = (TKey)(object)IdGenerateExtension.GenerateId();
                }
            }

            await DbCollection.InsertManyAsync(entities);
            return true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            entity.ModifyUserId = LoginUserInfo?.UserId;
            entity.ModifyTime = DateTime.Now;
            await DbCollection.ReplaceOneAsync(a => Equals(a.Id, entity.Id), entity);
            return true;
        }

        public async Task<bool> UpdateAsync(IEnumerable<TEntity> entities)
        {
            // 定义批量操作列表
            var updates = new List<WriteModel<TEntity>>();
            foreach (var entity in entities)
            {
                entity.ModifyUserId = LoginUserInfo?.UserId;
                entity.ModifyTime = DateTime.Now;

                // 添加更新操作
                var filter = Builders<TEntity>.Filter.Where(a => Equals(a.Id, entity.Id));
                var update = Builders<TEntity>.Update
                    .Set(a => a.ModifyUserId, LoginUserInfo?.UserId)
                    .Set(a => a.ModifyTime, DateTime.Now);
                updates.Add(new UpdateOneModel<TEntity>(filter, update));
            }

            await DbCollection.BulkWriteAsync(updates);
            return true;
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            // await DbCollection.DeleteOneAsync(a => Equals(a.Id, entity.Id));
            entity.DeletedUserId = LoginUserInfo?.UserId;
            entity.DeletedTime = DateTime.Now;
            entity.IsDelete = true;
            await DbCollection.ReplaceOneAsync(a => Equals(a.Id, entity.Id), entity);
            return true;
        }

        public async Task<bool> DeleteAsync(IEnumerable<TEntity> entities)
        {
            // 定义批量操作列表
            var updates = new List<WriteModel<TEntity>>();
            foreach (var entity in entities)
            {
                entity.ModifyUserId = LoginUserInfo?.UserId;
                entity.ModifyTime = DateTime.Now;

                // 添加更新操作
                var filter = Builders<TEntity>.Filter.Where(a => Equals(a.Id, entity.Id));
                var update = Builders<TEntity>.Update
                    .Set(a => a.DeletedUserId, LoginUserInfo?.UserId)
                    .Set(a => a.DeletedTime, DateTime.Now)
                    .Set(a => a.IsDelete, true);
                updates.Add(new UpdateOneModel<TEntity>(filter, update));
            }

            await DbCollection.BulkWriteAsync(updates);
            return true;
        }

        /// <summary>
        /// 硬删除
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ForcedDeleteAsync(TEntity entity)
        {
            await DbCollection.DeleteOneAsync(a => Equals(a.Id, entity.Id));
            return true;
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IReadOnlyList<TEntity>> GetAllListAsync()
        {
            return await DbCollection.AsQueryable().ToListAsync();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public virtual async Task<QueryPageDto<TDto>> QueryPageListAsync<TDto>(IQueryable<TEntity> query, int page, int pageSize)
        {
            var dbQuery = ToMongoQueryable(query);

            var total = await dbQuery.LongCountAsync();
            var dbResult = await dbQuery
                .OrderByDescending(a => a.CreatedTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new QueryPageDto<TDto>()
            {
                Total = total,
                Items = BaseMapper.Map<List<TEntity>, IReadOnlyList<TDto>>(dbResult)
            };
        }

        /// <summary>
        /// 分页查询(原始Entity)
        /// </summary>
        /// <returns></returns>
        public async Task<QueryPageDto<TEntity>> QueryPageListAsync(IQueryable<TEntity> query, int page, int pageSize)
        {
            var dbQuery = ToMongoQueryable(query);

            var total = await dbQuery.LongCountAsync();
            var dbResult = await dbQuery
                .OrderByDescending(a => a.CreatedTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new QueryPageDto<TEntity>()
            {
                Total = total,
                Items = dbResult
            };
        }

        /// <summary>
        /// 获取Queryable
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            await Task.CompletedTask;
            var query = DbCollection.AsQueryable();
            query = query.Where(a => a.IsDelete == false);
            return query;
        }

        /// <summary>
        /// 获取Queryable
        /// </summary>
        /// <remarks>
        /// 无过滤器
        /// </remarks>
        /// <returns></returns>
        public async Task<IQueryable<TEntity>> GetNoFilterQueryableAsync()
        {
            await Task.CompletedTask;
            var query = DbCollection.AsQueryable();
            return query;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var dbQuery = await GetQueryableAsync();
            return await ToMongoQueryable(dbQuery).AnyAsync(predicate);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<TEntity>> ToListAsync()
        {
            var dbQuery = await GetQueryableAsync();
            return await ToMongoQueryable(dbQuery).ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> ToListAsync(IQueryable<TEntity> query)
        {
            var dbQuery = ToMongoQueryable(query);
            return await dbQuery.ToListAsync();
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync(IQueryable<TEntity> query)
        {
            var dbQuery = ToMongoQueryable(query);
            return await dbQuery.CountAsync();
        }

        public async Task<long> LongCountAsync(IQueryable<TEntity> query)
        {
            var dbQuery = ToMongoQueryable(query);
            return await dbQuery.LongCountAsync();
        }

        internal IMongoQueryable<TEntity>? ToMongoQueryable(IQueryable<TEntity> query)
        {
            return query as IMongoQueryable<TEntity>;
        }
    }
}
