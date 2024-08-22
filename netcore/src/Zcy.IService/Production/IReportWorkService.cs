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
        /// 分页查询报工(管理员)
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageReportWorkForAdminDto>>> QueryPageReportWorkForAdminAsync(QueryPageReportWorkInput input);

        /// <summary>
        /// 分页查询报工(普通用户)
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageReportWorkForAdminDto>>> QueryPageReportWorkForNormalAsync(QueryPageReportWorkInput input);

        /// <summary>
        /// 创建报工
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateReportWorkAsync(CreateReportWorkInput input);

        /// <summary>
        /// 员工创建报工
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateReportWorkWithNormalAsync(CreateReportWorkWithNormalInput input);

        /// <summary>
        /// 批量创建报工
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchCreateReportWorkAsync(BatchCreateReportWorkInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input);

        /// <summary>
        /// 更新报工记录(仅更新实际结算价格)
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> UpdateReportWorkAsync(UpdateReportWorkInput input);

        /// <summary>
        /// 获取报工汇总
        /// </summary>
        /// <param name="input">query</param>
        /// <param name="isNormal">是否普通用户</param>
        /// <returns></returns>
        Task<KdyResult<GetReportWorkTotalsDto>> GetReportWorkTotalsAsync(QueryPageReportWorkInput input, bool isNormal = false);

        /// <summary>
        /// 通过员工报工
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> ApprovedReportWorkAsync(long reportWorkId);

        /// <summary>
        /// 驳回员工报工
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> RejectReportWorkAsync(long reportWorkId);

        /// <summary>
        /// 禁用员工报工
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BanReportWorkAsync(long reportWorkId);

        /// <summary>
        /// 更新报工信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> UpdateReportWorkInfoAsync(UpdateReportWorkInfoInput input);
    }
}
