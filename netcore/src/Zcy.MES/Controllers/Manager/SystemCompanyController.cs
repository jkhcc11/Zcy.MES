using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Dto.Company;
using Zcy.IService.Company;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 公司信息
    /// </summary>
    [Authorize(Roles = AuthorizationConst.NormalRoleName.SuperAdmin)]
    public class SystemCompanyController : BaseManagerController
    {
        private readonly ISystemCompanyService _systemCompanyService;

        public SystemCompanyController(ISystemCompanyService systemCompanyService)
        {
            _systemCompanyService = systemCompanyService;
        }

        /// <summary>
        /// 创建和更新 公司信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-and-update")]
        public async Task<KdyResult> CreateAndUpdateCompanyAsync(CreateAndUpdateCompanyInput input)
        {
            var result = await _systemCompanyService.CreateAndUpdateCompanyAsync(input);
            return result;
        }

        /// <summary>
        /// 查询公司信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("query")]
        public async Task<KdyResult<QueryPageDto<QueryPageCompanyDto>>> QueryPageCompanyAsync(
            [FromQuery] QueryPageCompanyInput input)
        {
            var result = await _systemCompanyService.QueryPageCompanyAsync(input);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<KdyResult> DeleteAsync(BatchOperationsInput input)
        {
            var result = await _systemCompanyService.BatchDeleteAsync(input);
            return result;
        }
    }
}
