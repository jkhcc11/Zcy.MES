using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.FinancialMemo;

namespace Zcy.IService.FinancialMemo
{
    /// <summary>
    /// 收支记录 服务接口
    /// </summary>
    public interface IIncomeRecordService : IZcyBaseService
    {
        /// <summary>
        /// 查询收支记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageIncomeRecordDto>>> QueryPageIncomeRecordAsync(QueryPageIncomeRecordInput input);

        /// <summary>
        /// 创建收支记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateIncomeRecordAsync(CreateIncomeRecordInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input);

        /// <summary>
        /// 获取收支记录汇总
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetIncomeRecordTotalsDto>> GetIncomeRecordTotalsAsync(QueryPageIncomeRecordInput input);
    }
}
