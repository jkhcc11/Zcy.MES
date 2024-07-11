using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.Dto.Products;
using Zcy.IService.Products;

namespace Zcy.MES.Controllers.Login
{
    /// <summary>
    /// 产品
    /// </summary>
    public class ProductController : BaseLoginController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// 普通用户-查询有效的产品
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-valid")]
        public async Task<KdyResult<List<QueryValidProductWithNormalDto>>> QueryValidProductWithNormalAsync(
            [FromQuery] QueryValidProductInput input)
        {
            var result = await _productService.QueryValidProductWithNormalAsync(input);
            return result;
        }

        /// <summary>
        /// 普通用户-获取产品工序
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-product-process/{id}")]
        public async Task<KdyResult<List<GetProductProcessWithNormalDto>>> GetProductProcessWithNormalAsync(long id)
        {
            var result = await _productService.GetProductProcessWithNormalAsync(id);
            return result;
        }
    }
}
