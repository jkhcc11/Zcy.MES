using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Zcy.BaseInterface.Entities;

namespace Zcy.MongoDB
{
    /// <summary>
    /// MongoDb
    /// </summary>
    public class QueryableExecuteWithMongoDb: IQueryableExecute
    {
        /// <summary>
        /// Any
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AnyAsync<TEntity>(IQueryable<TEntity> queryable)
        {
            var dbQuery = queryable as IMongoQueryable<TEntity>;
            return await dbQuery.AnyAsync();
        }

        /// <summary>
        /// ToList
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<TEntity>> ToListAsync<TEntity>(IQueryable<TEntity> queryable)
        {
            var dbQuery = queryable as IMongoQueryable<TEntity>;
            return await dbQuery.ToListAsync();
        }
    }
}
