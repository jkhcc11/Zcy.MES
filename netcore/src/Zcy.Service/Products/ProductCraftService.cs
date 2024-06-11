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
    /// 产品工艺 服务实现
    /// </summary>
    public class ProductCraftService : ZcyBaseService, IProductCraftService
    {
        private readonly IProductCraftRepository _productCraftRepository;
        private readonly IBaseRepository<SystemCompany, long> _systemCompanyRepository;

        public ProductCraftService(IProductCraftRepository productCraftRepository,
            IBaseRepository<SystemCompany, long> systemCompanyRepository)
        {
            _productCraftRepository = productCraftRepository;
            _systemCompanyRepository = systemCompanyRepository;
        }

        /// <summary>
        /// 分页查询产品工艺
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageProductCraftDto>>> QueryPageProductCraftAsync(QueryPageProductCraftInput input)
        {
            var query = await _productCraftRepository.GetQueryableAsync();
            query = query.CreateConditions(input);
            var result = await BaseQueryPageEntityAsync<ProductCraft, QueryPageProductCraftDto>(
                _productCraftRepository, query, input);
            if (result.Data.Items.Any())
            {
                await SetCompanyInfoAsync(result.Data.Items, _systemCompanyRepository);
            }

            return result;
        }

        /// <summary>
        /// 创建产品工艺
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateProductCraftAsync(CreateProductCraftInput input)
        {
            if (await _productCraftRepository.AnyAsync(a => a.CompanyId == LoginUserInfo.CompanyId &&
                                                            a.CraftName == input.CraftName))
            {
                return KdyResult.Error(KdyResultCode.Error, "工艺名称已存在，创建失败");
            }

            var entity = new ProductCraft(input.CraftName, input.CraftType, input.BillingType, input.UnitPrice)
            {
                Remark = input.Remark
            };
            await _productCraftRepository.CreateAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input)
        {
            var validCountQuery = await _productCraftRepository.GetQueryableAsync();
            validCountQuery = validCountQuery.Where(a => input.Ids.Contains(a.Id) &&
                                                         a.CraftStatus == PublicStatusEnum.Ban);
            var validCount = await _productCraftRepository.CountAsync(validCountQuery);
            if (validCount != input.Ids.Count)
            {
                return KdyResult.Error(KdyResultCode.Error, "操作失败，有效数据不一致");
            }

            return await BaseBatchDeleteAsync(_productCraftRepository, input);
        }

        /// <summary>
        /// 禁用|启用 产品工艺
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BanOrEnableAsync(long id)
        {
            var entity = await _productCraftRepository.GetEntityByIdAsync(id);
            if (entity.CraftStatus == PublicStatusEnum.Normal)
            {
                entity.Ban();
            }
            else
            {
                entity.Open();
            }

            await _productCraftRepository.UpdateAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 查询有效的产品工艺
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<QueryValidProductCraftDto>>> QueryValidProductCraftAsync(QueryValidProductCraftInput input)
        {
            var query = await _productCraftRepository.GetQueryableAsync();
            query = query.Where(a => a.CraftStatus == PublicStatusEnum.Normal);
            query = query.CreateConditions(input);
            var dbList = await _productCraftRepository.ToListAsync(query);
            var result = BaseMapper.Map<IReadOnlyList<ProductCraft>, List<QueryValidProductCraftDto>>(dbList);
            return KdyResult.Success(result);
        }
    }
}
