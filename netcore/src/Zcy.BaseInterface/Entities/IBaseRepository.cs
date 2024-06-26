﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Zcy.BaseInterface.Entities
{
    /// <summary>
    /// 通用仓储 接口
    /// </summary>
    public interface IBaseRepository<TEntity, in TKey>
    where TEntity : BaseEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <returns></returns>
        Task<TEntity?> FirstOrDefaultAsync(TKey keyId);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> FirstOrDefaultAsync(IQueryable<TEntity> queryable);

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <returns></returns>
        Task<TEntity> GetEntityByIdAsync(TKey keyId);

        Task<bool> CreateAsync(TEntity entity);
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        Task<bool> CreateAsync(List<TEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 软删除
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> DeleteAsync(IEnumerable<TEntity> entities);
        /// <summary>
        /// 硬删除
        /// </summary>
        /// <returns></returns>
        Task<bool> ForcedDeleteAsync(TEntity entity);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        Task<QueryPageDto<TDto>> QueryPageListAsync<TDto>(IQueryable<TEntity> query, QueryPageInput pageInput);

        /// <summary>
        /// 分页查询(原始Entity)
        /// </summary>
        /// <returns></returns>
        Task<QueryPageDto<TEntity>> QueryPageListAsync(IQueryable<TEntity> query, int page, int pageSize);

        /// <summary>
        /// 获取Queryable
        /// </summary>
        /// <remarks>
        /// 过滤已删除
        /// </remarks>
        /// <returns></returns>
        Task<IQueryable<TEntity>> GetQueryableAsync();

        /// <summary>
        /// 获取Queryable
        /// </summary>
        /// <remarks>
        /// 无过滤器
        /// </remarks>
        /// <returns></returns>
        Task<IQueryable<TEntity>> GetNoFilterQueryableAsync();

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<TEntity>> ToListAsync();
        Task<IReadOnlyList<TEntity>> ToListAsync(IQueryable<TEntity> query);

        /// <summary>
        /// 统计
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync(IQueryable<TEntity> query);
        Task<long> LongCountAsync(IQueryable<TEntity> query);
    }
}
