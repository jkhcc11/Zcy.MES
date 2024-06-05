using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.BaseInterface.Service;
using Zcy.Entity.Company;

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
        protected readonly IdGenerateExtension IdGenerateExtension;

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
            IdGenerateExtension = serviceProvider.GetService<IdGenerateExtension>() ??
                                  throw new ArgumentException("ZcyBaseService IdGenerateExtension is null");
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
            if (baseInput.OrderBy == null ||
                baseInput.OrderBy.Any() == false)
            {
                baseInput.OrderBy = new List<ZcyOrderConditions>()
                {
                    new()
                    {
                        Key = nameof(IBaseTimeKey.CreatedTime),
                        OrderBy = ZcyOrderByEnum.Desc
                    }
                };
            }

            var result = await entityRepository.QueryPageListAsync<TDto>(query, baseInput);

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

        /// <summary>
        /// 设置公司信息
        /// </summary>
        protected virtual async Task SetCompanyInfoAsync<TDto>(IReadOnlyList<TDto> items,
            IBaseRepository<SystemCompany, long> systemCompanyRepository)
        where TDto : class, IBaseCompanyDto
        {
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                return;
            }

            var dbCompany = await systemCompanyRepository.ToListAsync();
            foreach (var item in items)
            {
                item.CompanyName = dbCompany.FirstOrDefault(a => a.Id == item.CompanyId)?.CompanyName;
            }
        }

        /// <summary>
        /// 设置客户|供应商信息
        /// </summary>
        protected virtual async Task SetSupplierClientAsync<TDto>(IReadOnlyList<TDto> items,
            IBaseRepository<SupplierClient, long> supplierClientRepository)
            where TDto : class, IBaseSupplierClientDto
        {
            var clientIds = items.Select(a => a.SupplierClientId).ToArray();

            var dbSupplierClientQuery = await supplierClientRepository.GetQueryableAsync();
            dbSupplierClientQuery = dbSupplierClientQuery.Where(a => clientIds.Contains(a.Id));
            var dbCompany = await supplierClientRepository.ToListAsync(dbSupplierClientQuery);
            foreach (var item in items)
            {
                item.SupplierClientName = dbCompany.FirstOrDefault(a => a.Id == item.SupplierClientId)?.ClientName;
            }
        }
    }
}
