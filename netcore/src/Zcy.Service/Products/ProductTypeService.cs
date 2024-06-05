using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.Products;
using Zcy.Entity.Company;
using Zcy.Entity.Products;
using Zcy.IRepository.Products;
using Zcy.IService.Products;

namespace Zcy.Service.Products
{
    /// <summary>
    /// 产品分类 服务实现
    /// </summary>
    public class ProductTypeService : ZcyBaseService, IProductTypeService
    {
        private readonly IBaseRepository<ProductType, long> _productTypeRepository;
        private readonly IBaseRepository<SystemCompany, long> _systemCompanyRepository;
        private readonly IProductRepository _productRepository;

        public ProductTypeService(IBaseRepository<ProductType, long> productTypeRepository,
            IBaseRepository<SystemCompany, long> systemCompanyRepository,
            IProductRepository productRepository)
        {
            _productTypeRepository = productTypeRepository;
            _systemCompanyRepository = systemCompanyRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// 分页查询产品分类
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageProductTypeDto>>> QueryPageProductTypeAsync(QueryPageProductTypeInput input)
        {
            var query = await _productTypeRepository.GetQueryableAsync();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.CompanyId == LoginUserInfo.CompanyId);
            }

            query = query.CreateConditions(input);
            var result = await BaseQueryPageEntityAsync<ProductType, QueryPageProductTypeDto>(
                _productTypeRepository, query, input);
            if (result.Data.Items.Any())
            {
                await SetCompanyInfoAsync(result.Data.Items, _systemCompanyRepository);
            }

            return result;
        }

        /// <summary>
        /// 创建|更新 产品分类
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateUpdateProductTypeAsync(CreateUpdateProductTypeInput input)
        {
            if (input.Id.HasValue)
            {
                return await UpdateProductTypeAsync(input);
            }

            return await CreateProductTypeAsync(input);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input)
        {
            var validCountQuery = await _productTypeRepository.GetQueryableAsync();
            validCountQuery = validCountQuery.Where(a => input.Ids.Contains(a.Id) &&
                                                         a.TypeStatus == PublicStatusEnum.Ban);
            var validCount = await _productTypeRepository.CountAsync(validCountQuery);
            if (validCount != input.Ids.Count)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败，有效数据不一致");
            }

            if (await _productRepository.ExistByProductTypeAsync(input.Ids.ToArray()))
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败，分类存在产品，请调整");
            }

            return await BaseBatchDeleteAsync(_productTypeRepository, input);
        }

        /// <summary>
        /// 禁用|启用 产品分类
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BanOrEnableAsync(long id)
        {
            var entity = await _productTypeRepository.GetEntityByIdAsync(id);
            if (entity.TypeStatus == PublicStatusEnum.Normal)
            {
                entity.Ban();
            }
            else
            {
                entity.Open();
            }

            await _productTypeRepository.UpdateAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 查询有效的产品分类
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<QueryValidProductTypeDto>>> QueryValidProductTypeAsync()
        {
            var query = await _productTypeRepository.GetQueryableAsync();
            query = query.Where(a => a.TypeStatus == PublicStatusEnum.Normal);
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.CompanyId == LoginUserInfo.CompanyId);
            }

            var dbList = await _productTypeRepository.ToListAsync(query);
            var result = BaseMapper.Map<IReadOnlyList<ProductType>, List<QueryValidProductTypeDto>>(dbList);
            return KdyResult.Success(result);
        }

        #region 私有
        private async Task<KdyResult> CreateProductTypeAsync(CreateUpdateProductTypeInput input)
        {
            if (await _productTypeRepository.AnyAsync(a => a.CompanyId == LoginUserInfo.CompanyId &&
                                                       a.TypeName == input.TypeName))
            {
                return KdyResult.Error(KdyResultCode.Error, "产品分类已存在，创建失败");
            }

            var entity = new ProductType(input.TypeName);
            await _productTypeRepository.CreateAsync(entity);
            return KdyResult.Success();
        }

        private async Task<KdyResult> UpdateProductTypeAsync(CreateUpdateProductTypeInput input)
        {
            var entity = await _productTypeRepository.FirstOrDefaultAsync(input.Id.Value);
            if (entity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "Id参数无效");
            }

            if (await _productTypeRepository.AnyAsync(a => a.CompanyId == LoginUserInfo.CompanyId &&
                                                           a.TypeName == input.TypeName &&
                                                           a.Id != input.Id))
            {
                return KdyResult.Error(KdyResultCode.Error, "产品分类已存在，修改失败");
            }

            entity.TypeName = input.TypeName;
            await _productTypeRepository.UpdateAsync(entity);
            return KdyResult.Success();
        }
        #endregion
    }
}
