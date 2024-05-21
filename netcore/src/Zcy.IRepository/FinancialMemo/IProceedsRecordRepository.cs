using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.FinancialMemo;

namespace Zcy.IRepository.FinancialMemo
{
    /// <summary>
    /// 收款记录 仓储接口
    /// </summary>
    public interface IProceedsRecordRepository : IBaseRepository<ProceedsRecord, long>
    {
        /// <summary>
        /// 根据账户类型查询收款记录分组
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ProceedsRecordGroupTemp>> QueryProceedsRecordByAccountTypeAsync(IQueryable<ProceedsRecord> query);
    }
}
