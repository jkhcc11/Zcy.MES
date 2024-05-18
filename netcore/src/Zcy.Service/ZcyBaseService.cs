using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.BaseInterface.Service;

namespace Zcy.Service
{
    /// <summary>
    /// 基础服务
    /// </summary>
    public abstract class ZcyBaseService : IZcyBaseService
    {
        public IServiceCollection ServiceCollection { get; set; }
        public IConfiguration Configuration { get; set; }
        protected readonly ILoginUserInfo LoginUserInfo;
        protected readonly IMapper BaseMapper;

        protected ZcyBaseService()
        {
            ServiceCollection = AuthorizationConst.ServiceCollection;
            var serviceProvider = ServiceCollection.BuildServiceProvider();
            LoginUserInfo = serviceProvider.GetService<ILoginUserInfo>() ??
                            throw new ArgumentException("ZcyBaseService LoginUserInfo is null");
            BaseMapper = serviceProvider.GetService<IMapper>() ??
                         throw new ArgumentException("ZcyBaseService BaseMapper is null");
            Configuration = serviceProvider.GetService<IConfiguration>() ??
                           throw new ArgumentException("ZcyBaseService Configuration is null");
        }

        /// <summary>
        /// 查询公司列表
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<KdyResult<QueryPageDto<TDto>>> BaseQueryPageEntityAsync<TEntity, TDto>(
            IBaseRepository<TEntity, long> entityRepository,
            IQueryable<TEntity> query, QueryPageInput baseInput)
            where TEntity : BaseEntity<long>
        {
            var result = await entityRepository.QueryPageListAsync<TDto>(query, baseInput.Page,
                baseInput.PageSize);

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 基础 批量删除
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<KdyResult> BaseBatchDeleteAsync<TEntity>(IBaseRepository<TEntity, long> entityRepository,
            BatchOperationsInput input) where TEntity : BaseEntity<long>
        {
            var query = await entityRepository.GetQueryableAsync();
            query = query.Where(a => input.Ids.Contains(a.Id));
            var dbList = await entityRepository.ToListAsync(query);
            if (dbList.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "无有效数据");
            }

            await entityRepository.DeleteAsync(dbList);
            return KdyResult.Success();
        }
    }
}
