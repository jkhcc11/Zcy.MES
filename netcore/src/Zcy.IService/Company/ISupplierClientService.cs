using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.Company;

namespace Zcy.IService.Company
{
    /// <summary>
    /// 供应商|客户信息 服务接口
    /// </summary>
    public interface ISupplierClientService : IZcyBaseService
    {
        /// <summary>
        /// 查询供应商客户信息列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageSupplierClientDto>>> QueryPageSupplierClientAsync(QueryPageSupplierClientInput input);

        /// <summary>
        /// 创建和更新供应商客户信息
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateSupplierClientAsync(CreateAndUpdateSupplierClientInput input);

        /// <summary>
        /// 获取已启用的客户信息
        /// </summary>
        /// <remarks>
        ///  超管获取所有 其他仅获取当前公司
        /// </remarks>
        /// <returns></returns>
        Task<KdyResult<List<GetValidSupplierClientDto>>> GetValidSupplierClientAsync();

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input);

        /// <summary>
        /// 禁用
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BanClientAsync(long id);
    }
}
