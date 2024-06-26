using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.Products;

namespace Zcy.IService.Products
{
    /// <summary>
    /// 产品 服务接口
    /// </summary>
    public interface IProductService : IZcyBaseService
    {
        /// <summary>
        /// 分页查询产品
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageProductDto>>> QueryPageProductAsync(QueryPageProductInput input);

        /// <summary>
        /// 创建|更新产品
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateProductAsync(CreateAndUpdateProductInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteAsync(long productId);

        /// <summary>
        /// 禁用|启用 产品
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BanOrEnableAsync(long id);

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetProductDetailDto>> GetProductDetailAsync(long id);

        /// <summary>
        /// 获取产品详情（级联方式）
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<GetProductDetailCascadeDto>> GetProductDetailCascadeAsync(long id);

        /// <summary>
        /// 查询有效的产品
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<QueryValidProductDto>>> QueryValidProductAsync(QueryValidProductInput input);

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CopyAsync(long productId);
    }
}
