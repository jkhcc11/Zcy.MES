using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.Products;

namespace Zcy.IService.Products
{
    /// <summary>
    /// 产品工艺 服务接口
    /// </summary>
    public interface IProductCraftService : IZcyBaseService
    {
        /// <summary>
        /// 分页查询产品工艺
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageProductCraftDto>>> QueryPageProductCraftAsync(QueryPageProductCraftInput input);

        /// <summary>
        /// 创建产品工艺
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateProductCraftAsync(CreateProductCraftInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input);

        /// <summary>
        /// 禁用|启用 产品工艺
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BanOrEnableAsync(long id);

        /// <summary>
        /// 查询有效的产品工艺
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<QueryValidProductCraftDto>>> QueryValidProductCraftAsync(QueryValidProductCraftInput input);
    }
}
