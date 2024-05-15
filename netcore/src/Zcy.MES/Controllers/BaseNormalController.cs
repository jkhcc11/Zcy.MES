using Microsoft.AspNetCore.Mvc;

namespace Zcy.MES.Controllers
{
    /// <summary>
    /// 默认Controller基类
    /// </summary>
    [Route("api/mes-normal/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "normal")]
    public abstract class BaseNormalController : Controller
    {

    }
}
