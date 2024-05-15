using Zcy.BaseInterface.Entities;

namespace Zcy.MongoDB
{
    /// <summary>
    /// 通用仓储 实现
    /// </summary>
    /// <typeparam name="TEntity">数据库实体类</typeparam>
    /// <typeparam name="TKey">主键</typeparam>
    public class CommonRepository<TEntity, TKey> : BaseMongodbRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey> where TKey : struct
    {
        public CommonRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {

        }
    }
}
