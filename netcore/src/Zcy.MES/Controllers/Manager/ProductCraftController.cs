using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.Dto.Products;
using Zcy.IService.Products;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 产品工艺
    /// </summary>
    [Authorize(Roles = AuthorizationConst.NormalRoleName.Boss)]
    public class ProductCraftController : BaseManagerController
    {
        private readonly IProductCraftService _productCraftService;

        public ProductCraftController(IProductCraftService productCraftService)
        {
            _productCraftService = productCraftService;
        }

        /// <summary>
        /// 创建产品工艺
        /// </summary>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<KdyResult> CreateProductCraftAsync(CreateProductCraftInput input)
        {
            var result = await _productCraftService.CreateProductCraftAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询产品工艺
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageProductCraftDto>>> QueryPageProductCraftAsync(
            [FromQuery] QueryPageProductCraftInput input)
        {
            var result = await _productCraftService.QueryPageProductCraftAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<KdyResult> DeleteAsync(BatchOperationsInput input)
        {
            var result = await _productCraftService.BatchDeleteAsync(input);
            return result;
        }

        /// <summary>
        /// 禁用|启用 产品工艺
        /// </summary>
        /// <returns></returns>
        [HttpDelete("ban-or-enable/{id}")]
        public async Task<KdyResult> BanOrEnableAsync(long id)
        {
            var result = await _productCraftService.BanOrEnableAsync(id);
            return result;
        }

        /// <summary>
        /// 查询有效的产品工艺
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-valid")]
        public async Task<KdyResult<List<QueryValidProductCraftDto>>> QueryValidProductCraftAsync(
            [FromQuery] QueryValidProductCraftInput input)
        {
            var result = await _productCraftService.QueryValidProductCraftAsync(input);
            return result;
        }
    }
}
