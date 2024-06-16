using Microsoft.Extensions.Logging;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.Production;
using Zcy.Entity.Company;
using Zcy.Entity.Production;
using Zcy.Entity.Products;
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
            query = query.CreateConditions(input);
            var startTime = DateTime.Today.AddDays(-30);
            var endTime = DateTime.Today;
            if (input.StartTime.HasValue)
            {
                startTime = input.StartTime.Value.Date;
            }

            if (input.EndTime.HasValue)
            {
                endTime = input.EndTime.Value.Date;
            }

            query = query.Where(a => a.ReportWorkDate >= startTime &&
                                     a.ReportWorkDate <= endTime);
            var result = await BaseQueryPageEntityAsync<ReportWork, QueryPageReportWorkDto>(
                _reportWorkRepository, query, input);
            if (result.Data.Items.Any() == false)
            {
                return result;
            }

            await SetCompanyInfoAsync(result.Data.Items, _systemCompanyRepository);
            return result;
        }

        /// <summary>
        /// 分页查询报工(管理员)
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageReportWorkForAdminDto>>> QueryPageReportWorkForAdminAsync(
            QueryPageReportWorkInput input)
        {
            var query = await _reportWorkRepository.GetQueryableAsync();
            query = query.CreateConditions(input);
            var startTime = DateTime.Today.AddDays(-30);
            var endTime = DateTime.Today;
            if (input.StartTime.HasValue)
            {
                startTime = input.StartTime.Value.Date;
            }

            if (input.EndTime.HasValue)
            {
                endTime = input.EndTime.Value.Date;
            }

            query = query.Where(a => a.ReportWorkDate >= startTime &&
                                     a.ReportWorkDate <= endTime);
            var result = await BaseQueryPageEntityAsync<ReportWork, QueryPageReportWorkForAdminDto>(
                _reportWorkRepository, query, input);
            if (result.Data.Items.Any() == false)
            {
                return result;
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

            var employeeEntity = await _systemUserRepository.FirstOrDefaultAsync(input.EmployeeId);
            if (employeeEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "创建失败，员工不存在");
            }

            var productProcess = await _productRepository.GetProductProcessesAsync(input.ProductProcessId);
            if (productProcess == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "创建失败，无效产品工序");
            }

            if (productProcess.Product == null ||
                productProcess.ProductCraft == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "创建失败，无效产品工序-无效工艺");
            }

            var entity = new ReportWork(input.EmployeeId,
                employeeEntity.UserNick, reportWorkDate,
                input.ProductProcessId, input.WordDuration)
            {
                Remark = input.Remark
            };
            entity.SetProductProcessInfo(productProcess.ProcessingPrice,
                productProcess.ProductCraft.UnitPrice,
                productProcess.ProductCraft.BillingType,
                productProcess.Product.ProductName,
                productProcess.ProductCraft.CraftName);
            entity.TotalReceivablePrice();
            await _reportWorkRepository.CreateAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 批量创建报工
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchCreateReportWorkAsync(BatchCreateReportWorkInput input)
        {
            if (input.ReportWorkItems.Any() == false)
            {
                return KdyResult.Error(KdyResultCode.ParError, "参数错误，报工列表不能为空");
            }

            var reportWorkDate = input.ReportWorkDate ?? DateTime.Today;
            var companyId = LoginUserInfo.CompanyId;
            var productProcessIds = input.ReportWorkItems.Select(a => a.ProductProcessId).ToArray();

            if (await _reportWorkRepository.IsTodayExistReportWorkAsync(
                    companyId,
                    reportWorkDate,
                    input.EmployeeId,
                    productProcessIds))
            {
                return KdyResult.Error(KdyResultCode.Error, "创建失败，员工当天已存在报工,请检查是否添加重复");
            }

            var employeeEntity = await _systemUserRepository.FirstOrDefaultAsync(input.EmployeeId);
            if (employeeEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "创建失败，员工不存在");
            }

            var productProcess = await _productRepository.GetProductProcessesAsync(productProcessIds);
            if (productProcess.Count != productProcessIds.Length)
            {
                return KdyResult.Error(KdyResultCode.Error, "创建失败，存在无效产品工序，请刷新页面后重试");
            }

            var entities = new List<ReportWork>();
            foreach (var item in input.ReportWorkItems)
            {
                var entity = new ReportWork(input.EmployeeId,
                    employeeEntity.UserNick, reportWorkDate,
                    item.ProductProcessId, item.WordDuration)
                {
                    Remark = item.Remark
                };

                var currentDbProductProcess = productProcess.FirstOrDefault(a => a.Id == item.ProductProcessId);
                if (currentDbProductProcess == null ||
                    currentDbProductProcess.ProductCraft == null ||
                    currentDbProductProcess.Product == null)
                {
                    BaseLogger.LogError("创建报工失败，产品工序Id:{productProcessId}，无效", item.ProductProcessId);
                    return KdyResult.Error(KdyResultCode.Error, "创建失败，存在无效产品工序，请刷新页面后重试");
                }

                entity.SetProductProcessInfo(currentDbProductProcess.ProcessingPrice,
                    currentDbProductProcess.ProductCraft.UnitPrice,
                    currentDbProductProcess.ProductCraft.BillingType,
                    currentDbProductProcess.Product.ProductName,
                    currentDbProductProcess.ProductCraft.CraftName);
                entity.TotalReceivablePrice();
                entities.Add(entity);
            }

            await _reportWorkRepository.CreateAsync(entities);
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

        /// <summary>
        /// 获取报工汇总
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetReportWorkTotalsDto>> GetReportWorkTotalsAsync(QueryPageReportWorkInput input)
        {
            var query = await _reportWorkRepository.GetQueryableAsync();
            query = query.CreateConditions(input);
            //todo:量大可能有问题
            var dbList = await _reportWorkRepository.ToListAsync(query);
            var groupTemp = dbList
                .GroupBy(a => a.BillingType)
                .Select(a => new
                {
                    BillingType = a.Key,
                    WordDuration = a.Sum(b => b.WordDuration),
                    ActualSettlementPrice = a.Sum(b => b.ActualSettlementPrice),
                    ReceivableSettlementPrice = a.Sum(b => b.ReceivableSettlementPrice),
                }).ToList();

            var result = new GetReportWorkTotalsDto()
            {
                TotalActualSettlementPrice = groupTemp.Sum(a => a.ActualSettlementPrice),
                TotalReceivableSettlementPrice = groupTemp.Sum(a => a.ReceivableSettlementPrice),
                CountingWordDuration = groupTemp
                    .FirstOrDefault(a => a.BillingType == BillingTypeEnum.Counting)
                    ?.WordDuration,
                TimingWordDuration = groupTemp
                    .FirstOrDefault(a => a.BillingType == BillingTypeEnum.Timing)
                    ?.WordDuration,
            };
            if (LoginUserInfo.IsNotBossAndRoot)
            {
                //普通管理
                result.TotalActualSettlementPrice = default;
                result.TotalReceivableSettlementPrice = default;
            }

            return KdyResult.Success(result);
        }
    }
}
