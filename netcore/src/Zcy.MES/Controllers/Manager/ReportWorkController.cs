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

        public ReportWorkController(IReportWorkService reportWorkService)
        {
            _reportWorkService = reportWorkService;
        }

        /// <summary>
        /// 创建报工
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<KdyResult> CreateReportWorkAsync(CreateReportWorkInput input)
        {
            var result = await _reportWorkService.CreateReportWorkAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询报工
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
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
        [Authorize(Roles = AuthorizationConst.NormalRoleName.Boss)]
        public async Task<KdyResult> DeleteAsync(BatchOperationsInput input)
        {
            var result = await _reportWorkService.BatchDeleteAsync(input);
            return result;
        }

        /// <summary>
        /// 更新报工
        /// </summary>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.Boss)]
        public async Task<KdyResult> UpdateReportWorkAsync(UpdateReportWorkInput input)
        {
            var result = await _reportWorkService.UpdateReportWorkAsync(input);
            return result;
        }

        /// <summary>
        /// 更新报工
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-totals")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.Boss)]
        public async Task<KdyResult> GetReportWorkTotalsAsync([FromQuery] QueryPageReportWorkInput input)
        {
            var result = await _reportWorkService.GetReportWorkTotalsAsync(input);
            return result;
        }
    }
}
