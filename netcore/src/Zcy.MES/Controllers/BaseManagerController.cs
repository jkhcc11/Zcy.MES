using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zcy.BaseInterface;

namespace Zcy.MES.Controllers
{
    /// <summary>
    /// 管理Controller基类
    /// </summary>
    [Route("api/mes-manager/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "manager")]
    [Authorize(Policy = AuthorizationConst.NormalPolicyName.ManagerPolicy)]
    public abstract class BaseManagerController : Controller
    {

    }
}
