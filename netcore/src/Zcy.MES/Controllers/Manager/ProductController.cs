using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.Dto.Products;
using Zcy.IService.Products;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 产品
    /// </summary>
    [Authorize(Roles = AuthorizationConst.NormalRoleName.Boss + "," + AuthorizationConst.NormalRoleName.SuperAdmin)]
    public class ProductController : BaseManagerController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// 创建|更新 产品
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-or-update")]
        public async Task<KdyResult> CreateAndUpdateProductAsync(CreateAndUpdateProductInput input)
        {
            var result = await _productService.CreateAndUpdateProductAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询产品
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageProductDto>>> QueryPageProductAsync(
            [FromQuery] QueryPageProductInput input)
        {
            var result = await _productService.QueryPageProductAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<KdyResult> DeleteAsync(long id)
        {
            var result = await _productService.DeleteAsync(id);
            return result;
        }

        /// <summary>
        /// 禁用|启用 产品
        /// </summary>
        /// <returns></returns>
        [HttpDelete("ban-or-enable/{id}")]
        public async Task<KdyResult> BanOrEnableAsync(long id)
        {
            var result = await _productService.BanOrEnableAsync(id);
            return result;
        }

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-detail/{id}")]
        public async Task<KdyResult<GetProductDetailDto>> GetProductDetailAsync(long id)
        {
            var result = await _productService.GetProductDetailAsync(id);
            return result;
        }

        /// <summary>
        /// 查询有效的产品
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-valid")]
        public async Task<KdyResult<List<QueryValidProductDto>>> QueryValidProductAsync(
            [FromQuery] QueryValidProductInput input)
        {
            var result = await _productService.QueryValidProductAsync(input);
            return result;
        }
    }
}
