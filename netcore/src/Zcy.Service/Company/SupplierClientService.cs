using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.Company;
using Zcy.Entity.Company;
using Zcy.IService.Company;

namespace Zcy.Service.Company
{
    /// <summary>
    /// 供应商|客户信息 服务实现
    /// </summary>
    public class SupplierClientService : ZcyBaseService, ISupplierClientService
    {
        private readonly IBaseRepository<SupplierClient, long> _supplierClientRepository;
        private readonly IBaseRepository<SystemCompany, long> _companyRepository;

        public SupplierClientService(IBaseRepository<SupplierClient, long> supplierClientRepository,
            IBaseRepository<SystemCompany, long> companyRepository)
        {
            _supplierClientRepository = supplierClientRepository;
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// 查询供应商客户信息列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageSupplierClientDto>>> QueryPageSupplierClientAsync(QueryPageSupplierClientInput input)
        {
            var query = await _supplierClientRepository.GetQueryableAsync();
            query = query.CreateConditions(input);
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.CompanyId == LoginUserInfo.CompanyId);
            }

            var result = await BaseQueryPageEntityAsync<SupplierClient, QueryPageSupplierClientDto>(_supplierClientRepository,
                query, input);
            if (result.Data.Items.Any())
            {
                await SetCompanyInfoAsync(result.Data.Items, _companyRepository);
            }

            return result;
        }

        /// <summary>
        /// 创建和更新供应商客户信息
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateSupplierClientAsync(CreateAndUpdateSupplierClientInput input)
        {
            //if (LoginUserInfo.IsSuperAdmin == false &&
            //    input.CompanyId != LoginUserInfo.CompanyId)
            //{
            //    return KdyResult.Error(KdyResultCode.Error, "参数错误，公司不存在");
            //}

            if (await _companyRepository.AnyAsync(a => a.Id == input.CompanyId) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "参数错误，公司不存在");
            }

            if (input.Id.HasValue)
            {
                return await UpdateSupplierClientAsync(input);
            }

            return await CreateSupplierClientAsync(input);
        }

        /// <summary>
        /// 获取已启用的客户信息
        /// </summary>
        /// <remarks>
        ///  超管获取所有 其他仅获取当前公司
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult<List<GetValidSupplierClientDto>>> GetValidSupplierClientAsync(ClientTypeEnum clientType)
        {
            var query = await _supplierClientRepository.GetQueryableAsync();
            query = query.Where(a => a.ClientStatus == PublicStatusEnum.Normal &&
                                     a.ClientType == clientType);
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.CompanyId == LoginUserInfo.CompanyId);
            }

            var dbList = await _supplierClientRepository.ToListAsync(query);
            var result = BaseMapper.Map<IReadOnlyList<SupplierClient>, List<GetValidSupplierClientDto>>(dbList);
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input)
        {
            var validCountQuery = await _supplierClientRepository.GetQueryableAsync();
            validCountQuery = validCountQuery.Where(a => input.Ids.Contains(a.Id) &&
                                                         a.ClientStatus == PublicStatusEnum.Ban);
            var validCount = await _supplierClientRepository.CountAsync(validCountQuery);
            if (validCount != input.Ids.Count)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败，有效数据不一致");
            }

            return await BaseBatchDeleteAsync(_supplierClientRepository, input);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BanClientAsync(long id)
        {
            var entity = await _supplierClientRepository.GetEntityByIdAsync(id);
            entity.Ban();
            await _supplierClientRepository.UpdateAsync(entity);
            return KdyResult.Success();
        }

        #region 私有
        /// <summary>
        /// 创建客户信息
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> CreateSupplierClientAsync(CreateAndUpdateSupplierClientInput input)
        {
            if (await _supplierClientRepository.AnyAsync(a => a.ClientName == input.ClientName &&
                                                              a.CompanyId == input.CompanyId))
            {
                return KdyResult.Error(KdyResultCode.Error, "客户名称已存在");
            }

            var companyId = LoginUserInfo.CompanyId;
            if (LoginUserInfo.IsSuperAdmin &&
                input.CompanyId.HasValue)
            {
                companyId = input.CompanyId.Value;
            }

            var dbEntity = new SupplierClient(input.ClientName, input.ClientType)
            {
                Remark = input.Remark,
                PhoneNumber = input.PhoneNumber,
                CompanyId = companyId
            };

            //非超管强制默认
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                dbEntity.CompanyId = LoginUserInfo.CompanyId;
            }

            await _supplierClientRepository.CreateAsync(dbEntity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 修改客户信息
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> UpdateSupplierClientAsync(CreateAndUpdateSupplierClientInput input)
        {
            var dbEntity = await _supplierClientRepository.FirstOrDefaultAsync(input.Id.Value);
            if (dbEntity == null)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数错误");
            }

            if (await _supplierClientRepository.AnyAsync(a => a.ClientName == input.ClientName &&
                                                               a.CompanyId == input.CompanyId &&
                                                              a.Id != input.Id))
            {
                return KdyResult.Error(KdyResultCode.Error, "客户名称已存在");
            }

            dbEntity.SetClientName(input.ClientName);
            dbEntity.PhoneNumber = input.PhoneNumber;
            dbEntity.Remark = input.Remark;
            if (input.ClientStatus == PublicStatusEnum.Normal)
            {
                dbEntity.Open();
            }
            else
            {
                dbEntity.Ban();
            }

            await _supplierClientRepository.UpdateAsync(dbEntity);
            return KdyResult.Success();
        }
        #endregion
    }
}
