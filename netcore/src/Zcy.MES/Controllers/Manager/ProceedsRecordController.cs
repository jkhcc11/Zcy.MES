using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.IService.FinancialMemo;
using Zcy.Dto.FinancialMemo;
using Microsoft.AspNetCore.Authorization;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 收款记录
    /// </summary>
    [Authorize(Roles = AuthorizationConst.NormalRoleName.BossAndRoot)]
    public class ProceedsRecordController : BaseManagerController
    {
        private readonly IProceedsRecordService _proceedsRecordService;

        public ProceedsRecordController(IProceedsRecordService proceedsRecordService)
        {
            _proceedsRecordService = proceedsRecordService;
        }

        /// <summary>
        /// 创建收款记录
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        public async Task<KdyResult> CreateProceedsRecordAsync(CreateProceedsRecordInput input)
        {
            var result = await _proceedsRecordService.CreateProceedsRecordAsync(input);
            return result;
        }

        /// <summary>
        /// 查询收款记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageProceedsRecordDto>>> QueryPageProceedsRecordAsync(
            [FromQuery] QueryPageProceedsRecordInput input)
        {
            var result = await _proceedsRecordService.QueryPageProceedsRecordAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<KdyResult> DeleteAsync(BatchOperationsInput input)
        {
            var result = await _proceedsRecordService.BatchDeleteAsync(input);
            return result;
        }

        /// <summary>
        /// 获取收款汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-totals")]
        public async Task<KdyResult<GetProceedsRecordTotalsDto>> GetProceedsRecordTotalsAsync(
            [FromQuery] QueryPageProceedsRecordInput input)
        {
            var result = await _proceedsRecordService.GetProceedsRecordTotalsAsync(input);
            return result;
        }
    }
}
