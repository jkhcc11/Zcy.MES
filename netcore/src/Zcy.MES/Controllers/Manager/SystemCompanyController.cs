using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Dto.Company;
using Zcy.Dto.User;
using Zcy.IService.Company;
using Zcy.IService.User;

namespace Zcy.MES.Controllers.Manager
{
    /// <summary>
    /// 公司信息
    /// </summary>
    public class SystemCompanyController : BaseManagerController
    {
        private readonly ISystemCompanyService _systemCompanyService;
        private readonly IUserService _userService;

        public SystemCompanyController(ISystemCompanyService systemCompanyService,
            IUserService userService)
        {
            _systemCompanyService = systemCompanyService;
            _userService = userService;
        }

        /// <summary>
        /// 创建和更新 公司信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-and-update")]
        [Authorize(Roles = AuthorizationConst.NormalRoleName.SuperAdmin)]
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
        [Authorize(Roles = AuthorizationConst.NormalRoleName.SuperAdmin)]
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
        [Authorize(Roles = AuthorizationConst.NormalRoleName.SuperAdmin)]
        public async Task<KdyResult> DeleteAsync(BatchOperationsInput input)
        {
            var result = await _systemCompanyService.BatchDeleteAsync(input);
            return result;
        }

        /// <summary>
        /// 有效员工
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-valid-employee")]
        public async Task<KdyResult<List<GetCurrentCompanyValidEmployeeDto>>> GetCurrentCompanyValidEmployeeAsync()
        {
            var result = await _userService.GetCurrentCompanyValidEmployeeAsync();
            return result;
        }
    }
}
