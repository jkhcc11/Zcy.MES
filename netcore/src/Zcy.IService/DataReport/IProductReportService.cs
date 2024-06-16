using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.DataReport;

namespace Zcy.IService.DataReport
{
    /// <summary>
    /// 产品报表 服务接口
    /// </summary>
    public interface IProductReportService : IZcyBaseService
    {
        /// <summary>
        /// 分页查询产品销售报表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageProductSaleReportDto>>> QueryPageProductSaleReportAsync(QueryPageProductSaleReportInput input);
    }
}
