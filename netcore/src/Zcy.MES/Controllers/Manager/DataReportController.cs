using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Dto.DataReport;
using Zcy.IService.DataReport;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 数据报表
    /// </summary>
    [Authorize(Roles = AuthorizationConst.NormalRoleName.BossAndRoot)]
    public class DataReportController : BaseManagerController
    {
        private readonly IProductReportService _productReportService;

        public DataReportController(IProductReportService productReportService)
        {
            _productReportService = productReportService;
        }

        /// <summary>
        /// 分页查询产品销售报表
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-product-sale")]
        public async Task<KdyResult<QueryPageDto<QueryPageProductSaleReportDto>>> QueryPageProductSaleReportAsync(
            [FromQuery] QueryPageProductSaleReportInput input)
        {
            var result = await _productReportService.QueryPageProductSaleReportAsync(input);
            return result;
        }
    }
}
