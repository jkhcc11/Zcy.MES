using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.IService.FinancialMemo;
using Zcy.Dto.FinancialMemo;
using Microsoft.AspNetCore.Authorization;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 收支记录
    /// </summary>
    [Authorize(Roles = AuthorizationConst.NormalRoleName.BossAndRoot)]
    public class IncomeRecordController : BaseManagerController
    {
        private readonly IIncomeRecordService _incomeRecordService;

        public IncomeRecordController(IIncomeRecordService incomeRecordService)
        {
            _incomeRecordService = incomeRecordService;
        }


        /// <summary>
        /// 创建收支记录
        /// </summary>
        /// <returns></returns>
        [HttpPut("create")]
        public async Task<KdyResult> CreateIncomeRecordAsync(CreateIncomeRecordInput input)
        {
            var result = await _incomeRecordService.CreateIncomeRecordAsync(input);
            return result;
        }

        /// <summary>
        /// 查询收支记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageIncomeRecordDto>>> QueryPageIncomeRecordAsync(
            [FromQuery] QueryPageIncomeRecordInput input)
        {
            var result = await _incomeRecordService.QueryPageIncomeRecordAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<KdyResult> DeleteAsync(BatchOperationsInput input)
        {
            var result = await _incomeRecordService.BatchDeleteAsync(input);
            return result;
        }

        /// <summary>
        /// 获取收支记录汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-totals")]
        public async Task<KdyResult<GetIncomeRecordTotalsDto>> GetIncomeRecordTotalsAsync(
            [FromQuery] QueryPageIncomeRecordInput input)
        {
            var result = await _incomeRecordService.GetIncomeRecordTotalsAsync(input);
            return result;
        }
    }
}
