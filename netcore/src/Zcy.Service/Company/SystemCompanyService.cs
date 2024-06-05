using Microsoft.Extensions.Caching.Distributed;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.Company;
using Zcy.Entity.Company;
using Zcy.IService.Company;

namespace Zcy.Service.Company
{
    /// <summary>
    /// 公司 服务实现
    /// </summary>
    public class SystemCompanyService : ZcyBaseService, ISystemCompanyService
    {
        private readonly IBaseRepository<SystemCompany, long> _systemCompanyRepository;
        private readonly IDistributedCache _distributedCache;

        public SystemCompanyService(IBaseRepository<SystemCompany, long> systemCompanyRepository,
            IDistributedCache distributedCache)
        {
            _systemCompanyRepository = systemCompanyRepository;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// 查询公司列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageCompanyDto>>> QueryPageCompanyAsync(QueryPageCompanyInput input)
        {
            var query = await _systemCompanyRepository.GetQueryableAsync();
            query = query.CreateConditions(input);

            return await BaseQueryPageEntityAsync<SystemCompany, QueryPageCompanyDto>(_systemCompanyRepository,
                query, input);
        }

        /// <summary>
        /// 创建和更新公司
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateCompanyAsync(CreateAndUpdateCompanyInput input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateCompanyAsync(input);
            }

            return await CreateCompanyAsync(input);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input)
        {
            return await BaseBatchDeleteAsync(_systemCompanyRepository, input);
        }

        /// <summary>
        /// 公司信息缓存
        /// </summary>
        /// <returns></returns>
        public async Task<CompanyCacheItem> GetCompanyCacheAsync(long companyId)
        {
            var result = await _distributedCache.GetOrCreateAsync(companyId.ToString(), async () =>
            {
                var dbEntity = await _systemCompanyRepository.FirstOrDefaultAsync(companyId);
                if (dbEntity == null)
                {
                    return new CompanyCacheItem()
                    {
                        CompanyId = companyId
                    };
                }

                return BaseMapper.Map<SystemCompany, CompanyCacheItem>(dbEntity);
            });

            return result;
        }

        #region 私有
        /// <summary>
        /// 创建公司信息
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> CreateCompanyAsync(CreateAndUpdateCompanyInput input)
        {
            if (await _systemCompanyRepository.AnyAsync(a => a.CompanyName == input.CompanyName ||
                                                           a.ShortName == input.ShortName))
            {
                return KdyResult.Error(KdyResultCode.Error, "公司名称已存在");
            }

            var dbEntity = new SystemCompany(input.CompanyName, input.ShortName)
            {
                Remark = input.Remark,
                CompanyShowName = input.CompanyShowName
            };
            await _systemCompanyRepository.CreateAsync(dbEntity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> UpdateCompanyAsync(CreateAndUpdateCompanyInput input)
        {
            var dbEntity = await _systemCompanyRepository.FirstOrDefaultAsync(input.Id.Value);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数错误");
            }

            if (await _systemCompanyRepository.AnyAsync(a => (a.CompanyName == input.CompanyName ||
                                                             a.ShortName == input.ShortName) &&
                                                             a.Id != input.Id))
            {
                return KdyResult.Error(KdyResultCode.Error, "公司名称已存在");
            }

            dbEntity.CompanyShowName = input.CompanyShowName;
            dbEntity.Remark = input.Remark;
            await _systemCompanyRepository.UpdateAsync(dbEntity);
            return KdyResult.Success();
        }
        #endregion
    }
}
