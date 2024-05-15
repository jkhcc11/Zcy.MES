using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zcy.MES.Controllers
{
    /// <summary>
    /// 登录Controller基类
    /// </summary>
    [Route("api/mes-login/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "login")]
    [Authorize]
    public class BaseLoginController : Controller
    {
    }
}
