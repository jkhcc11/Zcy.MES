using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.Production;
using Zcy.Entity.Company;
using Zcy.Entity.Production;
using Zcy.IRepository.Production;
using Zcy.IRepository.Products;
using Zcy.IRepository.User;
using Zcy.IService.Production;

namespace Zcy.Service.Production
{
    /// <summary>
    /// 报工 服务实现
    /// </summary>
    public class ReportWorkService : ZcyBaseService, IReportWorkService
    {
        private readonly ISystemUserRepository _systemUserRepository;
        private readonly IProductRepository _productRepository;
        private readonly IReportWorkRepository _reportWorkRepository;
        private readonly IBaseRepository<SystemCompany, long> _systemCompanyRepository;

        public ReportWorkService(IReportWorkRepository reportWorkRepository,
            IBaseRepository<SystemCompany, long> systemCompanyRepository,
            ISystemUserRepository systemUserRepository,
            IProductRepository productRepository)
        {
            _reportWorkRepository = reportWorkRepository;
            _systemCompanyRepository = systemCompanyRepository;
            _systemUserRepository = systemUserRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// 分页查询报工
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageReportWorkDto>>> QueryPageReportWorkAsync(QueryPageReportWorkInput input)
        {
            var query = await _reportWorkRepository.GetQueryableAsync();
            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.CompanyId == LoginUserInfo.CompanyId);
            }

            query = query.CreateConditions(input);
            var result = await BaseQueryPageEntityAsync<ReportWork, QueryPageReportWorkDto>(
                _reportWorkRepository, query, input);
            if (result.Data.Items.Any() == false)
            {
                return result;
            }

            await SetCompanyInfoAsync(result.Data.Items, _systemCompanyRepository);
            if (LoginUserInfo is { IsSuperAdmin: false, IsBoss: false })
            {
                //清空价格
                foreach (var dtoItem in result.Data.Items)
                {
                    dtoItem.ReceivableSettlementPrice = default;
                    dtoItem.ActualSettlementPrice = default;
                    dtoItem.ProcessingPrice = default;
                    dtoItem.UnitPrice = default;
                }
            }

            return result;
        }

        /// <summary>
        /// 创建报工
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateReportWorkAsync(CreateReportWorkInput input)
        {
            var reportWorkDate = input.ReportWorkDate ?? DateTime.Today;
            var companyId = LoginUserInfo.CompanyId;
            if (await _reportWorkRepository.IsTodayExistReportWorkAsync(
                    companyId,
                    reportWorkDate,
                    input.EmployeeId,
                    input.ProductProcessId))
            {
                return KdyResult.Error(KdyResultCode.Error, "创建失败，员工当天已存在报工");
            }

            if (await _systemUserRepository.AnyAsync(a => a.Id == input.EmployeeId &&
                                                          a.CompanyId == companyId) == false)
            {
                return KdyResult.Error(KdyResultCode.Error, "创建失败，员工不存在");
            }

            var productProcess = await _productRepository.GetProductProcessesAsync(input.ProductProcessId);
            if (productProcess == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "创建失败，无效产品工序");
            }

            if (productProcess.ProductCraft == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "创建失败，无效产品工序-无效工艺");
            }

            var entity = new ReportWork(input.EmployeeId, reportWorkDate,
                input.ProductProcessId, input.WordDuration)
            {
                Remark = input.Remark
            };
            entity.SetProductProcessInfo(productProcess.ProcessingPrice,
                productProcess.ProductCraft.UnitPrice,
                productProcess.ProductCraft.BillingType);
            entity.TotalReceivablePrice();
            await _reportWorkRepository.CreateAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input)
        {
            return await BaseBatchDeleteAsync(_reportWorkRepository, input);
        }

        /// <summary>
        /// 更新报工记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> UpdateReportWorkAsync(UpdateReportWorkInput input)
        {
            if (string.IsNullOrEmpty(input.Remark) &&
                input.ActualSettlementPrice.HasValue == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "修改失败，无效参数");
            }

            var entity = await _reportWorkRepository.FirstOrDefaultAsync(input.Id);
            if (entity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "修改失败，无效报工记录");
            }

            if (string.IsNullOrEmpty(input.Remark) == false)
            {
                entity.Remark = input.Remark;
            }

            if (input.ActualSettlementPrice.HasValue)
            {
                entity.SetActualSettlementPrice(input.ActualSettlementPrice.Value);
            }

            await _reportWorkRepository.UpdateAsync(entity);
            return KdyResult.Success();
        }
    }
}
