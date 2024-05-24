using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.Service;
using Zcy.Dto.Products;

namespace Zcy.IService.Products
{
    /// <summary>
    /// 产品分类 服务接口
    /// </summary>
    public interface IProductTypeService : IZcyBaseService
    {
        /// <summary>
        /// 分页查询产品分类
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageProductTypeDto>>> QueryPageProductTypeAsync(QueryPageProductTypeInput input);

        /// <summary>
        /// 创建|更新 产品分类
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateUpdateProductTypeAsync(CreateUpdateProductTypeInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input);

        /// <summary>
        /// 禁用|启用 产品分类
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BanOrEnableAsync(long id);

        /// <summary>
        /// 查询有效的产品分类
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<QueryValidProductTypeDto>>> QueryValidProductTypeAsync();
    }
}
