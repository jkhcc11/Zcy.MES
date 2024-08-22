using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.Dto.Production;
using Zcy.IService.Production;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 报工
    /// </summary>
    public class ReportWorkController : BaseManagerController
    {
        private readonly IReportWorkService _reportWorkService;
        private readonly IReportWorkImportAndExportAppService _reportWorkImportAndExportAppService;

        public ReportWorkController(IReportWorkService reportWorkService,
            IReportWorkImportAndExportAppService reportWorkImportAndExportAppService)
        {
            _reportWorkService = reportWorkService;
            _reportWorkImportAndExportAppService = reportWorkImportAndExportAppService;
        }

        /// <summary>
        /// 创建报工
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        public async Task<KdyResult> CreateReportWorkAsync(CreateReportWorkInput input)
        {
            var result = await _reportWorkService.CreateReportWorkAsync(input);
            return result;
        }

        /// <summary>
        /// 批量创建报工
        /// </summary>
        /// <returns></returns>
        [HttpPut("batch-create")]
        public async Task<KdyResult> BatchCreateReportWorkAsync(BatchCreateReportWorkInput input)
        {
            var result = await _reportWorkService.BatchCreateReportWorkAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询报工(管理员)
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-for-admin")]
        public async Task<KdyResult<QueryPageDto<QueryPageReportWorkForAdminDto>>> QueryPageReportWorkForAdminAsync(
            [FromQuery] QueryPageReportWorkInput input)
        {
            var result = await _reportWorkService.QueryPageReportWorkForAdminAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询报工
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.BossAndRoot)]
        public async Task<KdyResult<QueryPageDto<QueryPageReportWorkDto>>> QueryPageProductCraftAsync(
            [FromQuery] QueryPageReportWorkInput input)
        {
            var result = await _reportWorkService.QueryPageReportWorkAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<KdyResult> DeleteAsync(BatchOperationsInput input)
        {
            var result = await _reportWorkService.BatchDeleteAsync(input);
            return result;
        }

        /// <summary>
        /// 更新报工(仅更新实际结算)
        /// </summary>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.BossAndRoot)]
        public async Task<KdyResult> UpdateReportWorkAsync(UpdateReportWorkInput input)
        {
            var result = await _reportWorkService.UpdateReportWorkAsync(input);
            return result;
        }

        /// <summary>
        /// 获取报工汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-totals")]
        public async Task<KdyResult<GetReportWorkTotalsDto>> GetReportWorkTotalsAsync([FromQuery] QueryPageReportWorkInput input)
        {
            var result = await _reportWorkService.GetReportWorkTotalsAsync(input);
            return result;
        }

        /// <summary>
        /// 通过
        /// </summary>
        /// <returns></returns>
        [HttpPost("approved/{reportWorkId}")]
        public async Task<KdyResult> ApprovedReportWorkAsync(long reportWorkId)
        {
            var result = await _reportWorkService.ApprovedReportWorkAsync(reportWorkId);
            return result;
        }

        /// <summary>
        /// 驳回
        /// </summary>
        /// <returns></returns>
        [HttpPost("reject/{reportWorkId}")]
        public async Task<KdyResult> RejectReportWorkAsync(long reportWorkId)
        {
            var result = await _reportWorkService.RejectReportWorkAsync(reportWorkId);
            return result;
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <returns></returns>
        [HttpDelete("ban/{reportWorkId}")]
        public async Task<KdyResult> BanReportWorkAsync(long reportWorkId)
        {
            var result = await _reportWorkService.BanReportWorkAsync(reportWorkId);
            return result;
        }

        /// <summary>
        /// 导出员工报工
        /// </summary>
        /// <returns></returns>
        [HttpGet("export-day-report-work")]
        public async Task<IActionResult> ExportDayReportWorkAsync([FromQuery] QueryPageReportWorkInput input)
        {
            var fileBytes = await _reportWorkImportAndExportAppService.ExportDayReportWorkAsync(input);
            var timeRange = BaseTimeRangeInputExt.GetTimeRange(input);
            var downFileName = $"{timeRange.sTime:yyyy年MM月dd日}至{timeRange.eTime:yyyy年MM月dd日} 员工汇总.xlsx";
            return File(fileBytes, ZcyMesConst.DownXlsxContextType, downFileName);
        }

        /// <summary>
        /// 导出产品报工
        /// </summary>
        /// <returns></returns>
        [HttpGet("export-product-report-work")]
        public async Task<IActionResult> ExportProductReportWorkAsync([FromQuery] QueryPageReportWorkInput input)
        {
            var fileBytes = await _reportWorkImportAndExportAppService.ExportProductReportWorkAsync(input);
            var timeRange = BaseTimeRangeInputExt.GetTimeRange(input);
            var downFileName = $"{timeRange.sTime:yyyy年MM月dd日}至{timeRange.eTime:yyyy年MM月dd日} 产品汇总.xlsx";
            return File(fileBytes, ZcyMesConst.DownXlsxContextType, downFileName);
        }

        /// <summary>
        /// 导出员工报工-横板
        /// </summary>
        /// <returns></returns>
        [HttpGet("export-day-report-work-horizontal")]
        public async Task<IActionResult> ExportDayReportWorkWithHorizontalAsync([FromQuery] QueryPageReportWorkInput input)
        {
            var fileBytes = await _reportWorkImportAndExportAppService.ExportDayReportWorkWithHorizontalAsync(input);
            var timeRange = BaseTimeRangeInputExt.GetTimeRange(input);
            var downFileName = $"{timeRange.sTime:yyyy年MM月dd日}至{timeRange.eTime:yyyy年MM月dd日} 员工汇总.xlsx";
            return File(fileBytes, ZcyMesConst.DownXlsxContextType, downFileName);
        }

        /// <summary>
        /// 导出员工报工-日期横板
        /// </summary>
        /// <returns></returns>
        [HttpGet("export-day-report-work-date-horizontal")]
        public async Task<IActionResult> ExportDayReportWorkWithDateHorizontalAsync([FromQuery] QueryPageReportWorkInput input)
        {
            var fileBytes = await _reportWorkImportAndExportAppService.ExportDayReportWorkWithDateHorizontalAsync(input);
            var timeRange = BaseTimeRangeInputExt.GetTimeRange(input);
            var downFileName = $"{timeRange.sTime:yyyy年MM月dd日}至{timeRange.eTime:yyyy年MM月dd日} 员工汇总.xlsx";
            return File(fileBytes, ZcyMesConst.DownXlsxContextType, downFileName);
        }

        /// <summary>
        /// 更新报工信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("update-report-work-info")]
        public async Task<KdyResult> UpdateReportWorkInfoAsync(UpdateReportWorkInfoInput input)
        {
            var result = await _reportWorkService.UpdateReportWorkInfoAsync(input);
            return result;
        }
    }
}
