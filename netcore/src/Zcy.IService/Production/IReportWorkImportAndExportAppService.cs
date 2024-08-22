using Zcy.BaseInterface.Service;
using Zcy.Dto.Production;

namespace Zcy.IService.Production
{
    /// <summary>
    /// 报工导入|导出 服务接口
    /// </summary>
    public interface IReportWorkImportAndExportAppService : IZcyBaseService
    {
        /// <summary>
        /// 导出员工报工
        /// </summary>
        /// <returns></returns>
        Task<byte[]> ExportDayReportWorkAsync(QueryPageReportWorkInput input);

        /// <summary>
        /// 导出产品报工
        /// </summary>
        /// <returns></returns>
        Task<byte[]> ExportProductReportWorkAsync(QueryPageReportWorkInput input);

        /// <summary>
        /// 导出员工报工（横向）
        /// </summary>
        /// <returns></returns>
        Task<byte[]> ExportDayReportWorkWithHorizontalAsync(QueryPageReportWorkInput input);

        /// <summary>
        /// 导出员工报工（日期横向）
        /// </summary>
        /// <returns></returns>
        Task<byte[]> ExportDayReportWorkWithDateHorizontalAsync(QueryPageReportWorkInput input);
    }
}
