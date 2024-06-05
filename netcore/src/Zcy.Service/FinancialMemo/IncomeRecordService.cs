using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.FinancialMemo;
using Zcy.Entity.Company;
using Zcy.Entity.FinancialMemo;
using Zcy.IService.FinancialMemo;

namespace Zcy.Service.FinancialMemo
{
    /// <summary>
    ///  收支记录 服务实现
    /// </summary>
    public class IncomeRecordService : ZcyBaseService, IIncomeRecordService
    {
        private readonly IBaseRepository<IncomeRecord, long> _incomeRecordRepository;
        private readonly IBaseRepository<SystemCompany, long> _systemCompanyRepository;

        public IncomeRecordService(IBaseRepository<IncomeRecord, long> incomeRecordRepository,
            IBaseRepository<SystemCompany, long> systemCompanyRepository)
        {
            _incomeRecordRepository = incomeRecordRepository;
            _systemCompanyRepository = systemCompanyRepository;
        }

        /// <summary>
        /// 查询收支记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageIncomeRecordDto>>> QueryPageIncomeRecordAsync(QueryPageIncomeRecordInput input)
        {
            var query = await BuildFilterAsync(input);
            var result = await BaseQueryPageEntityAsync<IncomeRecord, QueryPageIncomeRecordDto>(
                _incomeRecordRepository,
                query, input);
            if (result.Data.Items.Any())
            {
                await SetCompanyInfoAsync(result.Data.Items, _systemCompanyRepository);
            }

            return result;
        }

        /// <summary>
        /// 创建收支记录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateIncomeRecordAsync(CreateIncomeRecordInput input)
        {
            var recordData = DateTime.Today;
            if (input.RecordDate.HasValue)
            {
                recordData = input.RecordDate.Value.Date;
            }

            var managerUser = LoginUserInfo.UserNick;
            if (string.IsNullOrEmpty(input.ManagerUser) == false)
            {
                managerUser = input.ManagerUser;
            }

            if (await _incomeRecordRepository.AnyAsync(a => a.RecordName == input.RecordName &&
                                                            a.RecordDate == recordData))
            {
                return KdyResult.Error(KdyResultCode.Error, "当天相同记录已存在");
            }

            var entity = new IncomeRecord(input.IncomeType, input.AccountType,
                input.RecordName, input.Money, recordData, managerUser)
            {
                Remark = input.Remark
            };
            await _incomeRecordRepository.CreateAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input)
        {
            return await BaseBatchDeleteAsync(_incomeRecordRepository, input);
        }

        /// <summary>
        /// 获取收支记录汇总
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<KdyResult<GetIncomeRecordTotalsDto>> GetIncomeRecordTotalsAsync(QueryPageIncomeRecordInput input)
        {
            var query = await BuildFilterAsync(input);
            var dbList = await _incomeRecordRepository.ToListAsync(query);
            var incomeTypeGroup = dbList
                .GroupBy(a => a.AccountType)
                .Select(a => new AccountDetailItem()
                {
                    AccountType = a.Key,
                    InMoney = a.Where(b => b.IncomeType == IncomeTypeEnum.In).Sum(c => c.Money),
                    OutMoney = a.Where(b => b.IncomeType == IncomeTypeEnum.Out).Sum(c => c.Money),
                })
                .ToList();
            var result = new GetIncomeRecordTotalsDto()
            {
                InMoney = incomeTypeGroup.Sum(b => b.InMoney),
                OutMoney = incomeTypeGroup.Sum(b => b.OutMoney),
                AccountDetailList = incomeTypeGroup
            };
            return KdyResult.Success(result);
        }

        private async Task<IQueryable<IncomeRecord>> BuildFilterAsync(QueryPageIncomeRecordInput input)
        {
            DateTime startTime = DateTime.Now.AddDays(-30),
                endTime = DateTime.Now.AddDays(1);
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

            var query = await _incomeRecordRepository.GetQueryableAsync();

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
