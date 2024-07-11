using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.Dto.Production;
using Zcy.IService.Production;

namespace Zcy.MES.Controllers.Login
{
    /// <summary>
    /// 员工报工
    /// </summary>
    public class ReportWorkController : BaseLoginController
    {
        private readonly IReportWorkService _reportWorkService;
        public ReportWorkController(IReportWorkService reportWorkService)
        {
            _reportWorkService = reportWorkService;
        }

        /// <summary>
        /// 员工创建报工
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        public async Task<KdyResult> CreateReportWorkWithNormalAsync(CreateReportWorkWithNormalInput input)
        {
            var result = await _reportWorkService.CreateReportWorkWithNormalAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询报工(普通用户)
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageReportWorkForAdminDto>>> QueryPageReportWorkForNormalAsync(
            [FromQuery] QueryPageReportWorkInput input)
        {
            var result = await _reportWorkService.QueryPageReportWorkForNormalAsync(input);
            return result;
        }

        /// <summary>
        /// 获取报工汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-totals")]
        public async Task<KdyResult<GetReportWorkTotalsDto>> GetReportWorkTotalsAsync([FromQuery] QueryPageReportWorkInput input)
        {
            var result = await _reportWorkService.GetReportWorkTotalsAsync(input, true);
            return result;
        }
    }
}
