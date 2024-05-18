using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.Company;
using Zcy.Entity.Company;

namespace Zcy.IService.Company
{
    /// <summary>
    /// 公司 服务接口
    /// </summary>
    public interface ISystemCompanyService : IZcyBaseService
    {
        /// <summary>
        /// 查询公司列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageCompanyDto>>> QueryPageCompanyAsync(QueryPageCompanyInput input);

        /// <summary>
        /// 创建和更新公司
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateCompanyAsync(CreateAndUpdateCompanyInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input);

        /// <summary>
        /// 公司信息缓存
        /// </summary>
        /// <returns></returns>
        Task<CompanyCacheItem> GetCompanyCacheAsync(long companyId);
    }
}
