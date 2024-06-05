using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.FinancialMemo;
using Zcy.Entity.Company;
using Zcy.Entity.FinancialMemo;
using Zcy.IRepository.FinancialMemo;
using Zcy.IService.FinancialMemo;

namespace Zcy.Service.FinancialMemo
{
    /// <summary>
    ///  收款记录 服务实现
    /// </summary>
    public class ProceedsRecordService : ZcyBaseService, IProceedsRecordService
    {
        private readonly IProceedsRecordRepository _proceedsRecordRepository;
        private readonly IBaseRepository<SystemCompany, long> _systemCompanyRepository;
        private readonly IBaseRepository<SupplierClient, long> _supplierClientRepository;

        public ProceedsRecordService(IProceedsRecordRepository proceedsRecordRepository,
            IBaseRepository<SystemCompany, long> systemCompanyRepository,
            IBaseRepository<SupplierClient, long> supplierClientRepository)
        {
            _proceedsRecordRepository = proceedsRecordRepository;
            _systemCompanyRepository = systemCompanyRepository;
            _supplierClientRepository = supplierClientRepository;
        }

        /// <summary>
        /// 查询收款记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageProceedsRecordDto>>> QueryPageProceedsRecordAsync(QueryPageProceedsRecordInput input)
        {
            var query = await BuildFilterAsync(input);
            var result = await BaseQueryPageEntityAsync<ProceedsRecord, QueryPageProceedsRecordDto>(
                _proceedsRecordRepository,
                query, input);
            if (result.Data.Items.Any())
            {
                await SetCompanyInfoAsync(result.Data.Items, _systemCompanyRepository);
                await SetSupplierClientAsync(result.Data.Items, _supplierClientRepository);
            }

            return result;
        }

        /// <summary>
        /// 创建收款记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateProceedsRecordAsync(CreateProceedsRecordInput input)
        {
            var recordData = DateTime.Today;
            if (input.RecordDate.HasValue)
            {
                recordData = input.RecordDate.Value.Date;
            }

            //todo:这里需要确认 是不是一天同一个客户只有一个收款记录
            if (await _proceedsRecordRepository.AnyAsync(a => a.SupplierClientId == input.SupplierClientId &&
                                                              a.AccountType == input.AccountType &&
                                                              a.RecordDate == recordData))
            {
                return KdyResult.Error(KdyResultCode.Error, "客户当天相同记录已存在");
            }

            var entity = new ProceedsRecord(input.ProceedsRecordName, input.SupplierClientId,
                recordData, input.AccountType,
                input.Money)
            {
                Remark = input.Remark
            };
            await _proceedsRecordRepository.CreateAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input)
        {
            return await BaseBatchDeleteAsync(_proceedsRecordRepository, input);
        }

        /// <summary>
        /// 获取收款记录汇总
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<GetProceedsRecordTotalsDto>> GetProceedsRecordTotalsAsync(QueryPageProceedsRecordInput input)
        {
            //db分组
            var query = await BuildFilterAsync(input);
            var dbResult = await _proceedsRecordRepository.QueryProceedsRecordByAccountTypeAsync(query);

            var tempResult = dbResult
                .Select(a => new AccountDetailWithProceedsRecordItem()
                {
                    AccountType = a.AccountType,
                    Money = a.Money
                })
                .ToList();
            var result = new GetProceedsRecordTotalsDto()
            {
                AccountDetailList = tempResult
            };
            return KdyResult.Success(result);
        }

        private async Task<IQueryable<ProceedsRecord>> BuildFilterAsync(QueryPageProceedsRecordInput input)
        {
            DateTime startTime = DateTime.Now.AddDays(-30),
                endTime = DateTime.Now;
            if (input.StartTime is { })
            {
                startTime = input.StartTime.Value;
            }

            if (input.EndTime is { })
            {
                endTime = input.EndTime.Value;
            }

            if ((endTime - startTime).TotalDays > 366)
            {
                throw new ZcyCustomException("时间范围错误，最大一年范围");
            }

            var query = await _proceedsRecordRepository.GetQueryableAsync();

            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.CompanyId == LoginUserInfo.CompanyId);
            }

            query = query.CreateConditions(input);
            if (input.StartTime.HasValue)
            {
                query = query.Where(a => a.RecordDate >= startTime);
            }

            if (input.EndTime.HasValue)
            {
                query = query.Where(a => a.RecordDate <= endTime);
            }

            return query;
        }
    }
}
