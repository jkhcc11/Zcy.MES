using System.Linq.Dynamic.Core;
using Zcy.Entity.FinancialMemo;
using Zcy.IRepository.FinancialMemo;

namespace Zcy.MongoDB.FinancialMemo
{
    /// <summary>
    /// 收款记录 仓储实现
    /// </summary>
    public class ProceedsRecordRepository : BaseMongodbRepository<ProceedsRecord, long>, IProceedsRecordRepository
    {
        public ProceedsRecordRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {
        }

        /// <summary>
        /// 根据账户类型查询收款记录分组
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<ProceedsRecordGroupTemp>> QueryProceedsRecordByAccountTypeAsync(IQueryable<ProceedsRecord> query)
        {
            var mongoQuery = ToMongoQueryable(query);
            var result = await mongoQuery
                .GroupBy(a => a.AccountType)
                .Select(a => new ProceedsRecordGroupTemp()
                {
                    AccountType = a.Key,
                    Money = a.Sum(b => b.Money)
                })
                .ToDynamicListAsync<ProceedsRecordGroupTemp>();
            return result;
        }
    }
}
