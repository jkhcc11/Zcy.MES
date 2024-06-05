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
    /// 产品 服务实现
    /// </summary>
    public class ProductService : ZcyBaseService, IProductService
    {
        private readonly IProductCraftRepository _productCraftRepository;
        private readonly IBaseRepository<SystemCompany, long> _systemCompanyRepository;
        private readonly IBaseRepository<ProductType, long> _productTypeRepository;
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository,
            IBaseRepository<ProductType, long> productTypeRepository,
            IBaseRepository<SystemCompany, long> systemCompanyRepository,
            IProductCraftRepository productCraftRepository)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _systemCompanyRepository = systemCompanyRepository;
            _productCraftRepository = productCraftRepository;
        }

        /// <summary>
        /// 分页查询产品
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageProductDto>>> QueryPageProductAsync(QueryPageProductInput input)
        {
            var query = await _productRepository.GetQueryableAsync();
            var allProductTypeQuery = await _productTypeRepository.GetQueryableAsync();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.CompanyId == LoginUserInfo.CompanyId);
                allProductTypeQuery = allProductTypeQuery.Where(a => a.CompanyId == LoginUserInfo.CompanyId);
            }

            query = query.CreateConditions(input);
            var result = await BaseQueryPageEntityAsync<Product, QueryPageProductDto>(
                _productRepository, query, input);
            if (result.Data.Items.Any())
            {
                var allProductType = await _productTypeRepository.ToListAsync(allProductTypeQuery);
                foreach (var dtoItem in result.Data.Items)
                {
                    dtoItem.ProductTypeName =
                        allProductType.FirstOrDefault(a => a.Id == dtoItem.ProductTypeId)?.TypeName;
                }

                await SetCompanyInfoAsync(result.Data.Items, _systemCompanyRepository);
            }

            return result;
        }

        /// <summary>
        /// 创建|更新 产品
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateProductAsync(CreateAndUpdateProductInput input)
        {
            if (input.IsLoose == false &&
                input.SpecCount.HasValue == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "非散件，请填写规格数");
            }

            if (input.ProductType != ProductTypeEnum.Processing &&
                input.ProductProcesses != null &&
                input.ProductProcesses.Any())
            {
                return KdyResult.Error(KdyResultCode.Error, "非加工产品，不能有产品工序");
            }

            if (input.ProductType == ProductTypeEnum.Processing &&
                (input.ProductProcesses == null ||
                input.ProductProcesses.Any() == false))
            {
                return KdyResult.Error(KdyResultCode.Error, "加工产品，请添加产品工序");
            }


            if (await _productTypeRepository.AnyAsync(a => a.Id == input.ProductTypeId) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "产品分类不存在，修改失败");
            }

            if (input.Id.HasValue)
            {
                return await UpdateProductAsync(input);
            }

            return await CreateProductAsync(input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> DeleteAsync(long productId)
        {
            var entity = await _productRepository.FirstOrDefaultAsync(productId);
            if (entity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效Id");
            }

            if (entity.ProductStatus != PublicStatusEnum.Ban)
            {
                return KdyResult.Error(KdyResultCode.Error, "无效数据");
            }

            await _productRepository.DeleteAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 禁用|启用 产品
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BanOrEnableAsync(long id)
        {
            var entity = await _productRepository.GetEntityByIdAsync(id);
            if (entity.ProductStatus == PublicStatusEnum.Normal)
            {
                entity.Ban();
            }
            else
            {
                entity.Open();
            }

            await _productRepository.UpdateAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetProductDetailDto>> GetProductDetailAsync(long id)
        {
            var entity = await _productRepository.FirstOrDefaultAsync(id);
            if (entity == null)
            {
                return KdyResult.Error<GetProductDetailDto>(KdyResultCode.Error, "Id参数无效");
            }

            #region 加工产品
            if (entity.ProductType == ProductTypeEnum.Processing &&
                  entity.ProductProcesses != null)
            {
                var craftIds = entity.ProductProcesses.Select(a => a.CraftId).ToArray();
                var validCountQuery = await _productCraftRepository.GetQueryableAsync();
                validCountQuery = validCountQuery.Where(a => craftIds.Contains(a.Id) &&
                                                             a.CraftStatus == PublicStatusEnum.Normal);
                var validCraft = await _productCraftRepository.ToListAsync(validCountQuery);
                foreach (var item in entity.ProductProcesses)
                {
                    item.ProductCraft = validCraft.FirstOrDefault(a => a.Id == item.CraftId);
                }
            }
            #endregion


            var result = BaseMapper.Map<Product, GetProductDetailDto>(entity);
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 查询有效的产品
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<QueryValidProductDto>>> QueryValidProductAsync(QueryValidProductInput input)
        {
            var query = await _productRepository.GetQueryableAsync();
            query = query.Where(a => a.ProductStatus == PublicStatusEnum.Normal);
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.CompanyId == LoginUserInfo.CompanyId);
            }

            query = query.CreateConditions(input);
            var dbList = await _productRepository.ToListAsync(query);
            var result = BaseMapper.Map<IReadOnlyList<Product>, List<QueryValidProductDto>>(dbList);
            return KdyResult.Success(result);
        }

        #region 私有
        private async Task<KdyResult> CreateProductAsync(CreateAndUpdateProductInput input)
        {
            if (await _productRepository.AnyAsync(a => a.CompanyId == LoginUserInfo.CompanyId &&
                                                       a.ProductName == input.ProductName))
            {
                return KdyResult.Error(KdyResultCode.Error, "产品名称已存在，创建失败");
            }

            var entity = new Product(IdGenerateExtension.GenerateId(), input.ProductTypeId,
                input.ProductName, input.ProductType, input.IsLoose,
                input.Unit)
            {
                ProductRemark = input.ProductRemark,
                CompanyId = LoginUserInfo.CompanyId
            };

            if (entity.IsLoose)
            {
                entity.Spec = null;
                entity.SpecCount = 0;
            }
            else
            {
                entity.Spec = input.Spec;
                entity.SpecCount = input.SpecCount ?? 0;
            }

            if (input.ProductProcesses != null &&
                input.ProductProcesses.Any())
            {
                var craftIds = input.ProductProcesses.Select(a => a.CraftId).ToArray();
                var validCountQuery = await _productCraftRepository.GetQueryableAsync();
                validCountQuery = validCountQuery.Where(a => craftIds.Contains(a.Id) &&
                                                             a.CraftStatus == PublicStatusEnum.Normal);
                var validCount = await _productCraftRepository.CountAsync(validCountQuery);
                if (validCount != craftIds.Length)
                {
                    return KdyResult.Error(KdyResultCode.Error, "操作失败，产品工艺有效数据不一致");
                }

                entity.ProductProcesses = input.ProductProcesses
                    .Select(a => new ProductProcess(IdGenerateExtension.GenerateId(),
                        entity.Id,
                        a.CraftId)
                    {
                        ProcessingPrice = a.ProcessingPrice,
                        OrderBy = a.OrderBy
                    })
                    .ToList();
            }

            await _productRepository.CreateAsync(entity);
            return KdyResult.Success();
        }

        private async Task<KdyResult> UpdateProductAsync(CreateAndUpdateProductInput input)
        {
            var entity = await _productRepository.FirstOrDefaultAsync(input.Id.Value);
            if (entity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "Id参数无效");
            }

            if (await _productRepository.AnyAsync(a => a.CompanyId == LoginUserInfo.CompanyId &&
                                                       a.ProductName == input.ProductName &&
                                                       a.Id != input.Id))
            {
                return KdyResult.Error(KdyResultCode.Error, "产品名称已存在，修改失败");
            }

            if (entity.ProductStatus == PublicStatusEnum.Normal)
            {
                return KdyResult.Error(KdyResultCode.Error, "产品当前状态无法修改");
            }

            entity.SetProductTypeId(input.ProductTypeId);
            entity.SetProductName(input.ProductName);
            entity.SetProductType(input.ProductType);
            entity.SetUnit(input.Unit);
            entity.IsLoose = input.IsLoose;

            if (entity.IsLoose)
            {
                entity.Spec = null;
                entity.SpecCount = 0;
            }
            else
            {
                entity.Spec = input.Spec;
                entity.SpecCount = input.SpecCount ?? 0;
            }

            entity.ProductRemark = input.ProductRemark;

            //先删除旧的
            var oldProductProcess = entity.ProductProcesses;
            entity.ProductProcesses = null;
            if (input.ProductProcesses != null &&
                input.ProductProcesses.Any())
            {
                var craftIds = input.ProductProcesses.Select(a => a.CraftId).ToArray();
                var validCountQuery = await _productCraftRepository.GetQueryableAsync();
                validCountQuery = validCountQuery.Where(a => craftIds.Contains(a.Id) &&
                                                             a.CraftStatus == PublicStatusEnum.Normal);
                var validCount = await _productCraftRepository.CountAsync(validCountQuery);
                if (validCount != craftIds.Length)
                {
                    return KdyResult.Error(KdyResultCode.Error, "操作失败，产品工艺有效数据不一致");
                }

                entity.ProductProcesses = input.ProductProcesses
                    .Select(a => new ProductProcess(IdGenerateExtension.GenerateId(),
                        entity.Id,
                        a.CraftId)
                    {
                        ProcessingPrice = a.ProcessingPrice,
                        OrderBy = a.OrderBy
                    })
                    .ToList();
            }

            await _productRepository.UpdateAsync(entity, oldProductProcess);
            return KdyResult.Success();
        }
        #endregion
    }
}
