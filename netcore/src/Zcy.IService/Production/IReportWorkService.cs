using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Service;
using Zcy.Dto.Production;

namespace Zcy.IService.Production
{
    /// <summary>
    /// 报工 服务接口
    /// </summary>
    public interface IReportWorkService : IZcyBaseService
    {
        /// <summary>
        /// 分页查询报工
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageReportWorkDto>>> QueryPageReportWorkAsync(QueryPageReportWorkInput input);

        /// <summary>
        /// 创建报工
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateReportWorkAsync(CreateReportWorkInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input);

        /// <summary>
        /// 更新报工记录
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> UpdateReportWorkAsync(UpdateReportWorkInput input);

        /// <summary>
        /// 获取报工汇总
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetReportWorkTotalsDto>> GetReportWorkTotalsAsync(QueryPageReportWorkInput input);
    }
}
