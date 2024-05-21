using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.FinancialMemo;

namespace Zcy.IService.FinancialMemo
{
    /// <summary>
    /// 收款记录 服务接口
    /// </summary>
    public interface IProceedsRecordService : IZcyBaseService
    {
        /// <summary>
        /// 查询收款记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageProceedsRecordDto>>> QueryPageProceedsRecordAsync(QueryPageProceedsRecordInput input);

        /// <summary>
        /// 创建收款记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateProceedsRecordAsync(CreateProceedsRecordInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input);

        /// <summary>
        /// 获取收款记录汇总
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetProceedsRecordTotalsDto>> GetProceedsRecordTotalsAsync(QueryPageProceedsRecordInput input);
    }
}
