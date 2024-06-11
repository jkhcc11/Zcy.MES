using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Dto.Company;
using Zcy.Entity.Company;
using Zcy.IService.Company;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 客户信息
    /// </summary>
    public class SupplierClientController : BaseManagerController
    {
        private readonly ISupplierClientService _supplierClientService;

        public SupplierClientController(ISupplierClientService supplierClientService)
        {
            _supplierClientService = supplierClientService;
        }


        /// <summary>
        /// 创建和更新 客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-and-update")]
        public async Task<KdyResult> CreateAndUpdateSupplierClientAsync(CreateAndUpdateSupplierClientInput input)
        {
            var result = await _supplierClientService.CreateAndUpdateSupplierClientAsync(input);
            return result;
        }

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageSupplierClientDto>>> QueryPageSupplierClientAsync(
            [FromQuery] QueryPageSupplierClientInput input)
        {
            var result = await _supplierClientService.QueryPageSupplierClientAsync(input);
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
            var result = await _supplierClientService.BatchDeleteAsync(input);
            return result;
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <returns></returns>
        [HttpDelete("ban/{keyId}")]
        public async Task<KdyResult> BanClientAsync(long keyId)
        {
            var result = await _supplierClientService.BanClientAsync(keyId);
            return result;
        }

        /// <summary>
        /// 获取已启用的客户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-open-client")]
        public async Task<KdyResult<List<GetValidSupplierClientDto>>> GetValidSupplierClientAsync(ClientTypeEnum clientType)
        {
            var result = await _supplierClientService.GetValidSupplierClientAsync(clientType);
            return result;
        }
    }
}
