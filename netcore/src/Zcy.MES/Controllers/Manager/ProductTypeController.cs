using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface;
using Zcy.Dto.Products;
using Zcy.IService.Products;
using Microsoft.AspNetCore.Authorization;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 产品分类
    /// </summary>
    public class ProductTypeController : BaseManagerController
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        /// <summary>
        /// 创建产品分类
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-or-update")]
        public async Task<KdyResult> CreateUpdateProductTypeAsync(CreateUpdateProductTypeInput input)
        {
            var result = await _productTypeService.CreateUpdateProductTypeAsync(input);
            return result;
        }

        /// <summary>
        /// 分页查询产品分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageProductTypeDto>>> QueryPageProductTypeAsync(
            [FromQuery] QueryPageProductTypeInput input)
        {
            var result = await _productTypeService.QueryPageProductTypeAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.BossAndRoot)]
        public async Task<KdyResult> DeleteAsync(BatchOperationsInput input)
        {
            var result = await _productTypeService.BatchDeleteAsync(input);
            return result;
        }

        /// <summary>
        /// 禁用|启用 产品分类
        /// </summary>
        /// <returns></returns>
        [HttpDelete("ban-or-enable/{id}")]
        public async Task<KdyResult> BanOrEnableAsync(long id)
        {
            var result = await _productTypeService.BanOrEnableAsync(id);
            return result;
        }

        /// <summary>
        /// 查询有效的产品分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("query-valid")]
        public async Task<KdyResult<List<QueryValidProductTypeDto>>> QueryValidProductTypeAsync()
        {
            var result = await _productTypeService.QueryValidProductTypeAsync();
            return result;
        }
    }
}
