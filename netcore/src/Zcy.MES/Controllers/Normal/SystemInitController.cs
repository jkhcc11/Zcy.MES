using Microsoft.AspNetCore.Mvc;
using Zcy.IService.SysBaseInfo;
using Zcy.IService.User;

namespace Zcy.MES.Controllers.Normal
{
    public class SystemInitController : BaseNormalController
    {
        private readonly ISystemRoleService _roleService;
        private readonly IUserService _userService;

        public SystemInitController(ISystemRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        [HttpGet("init")]
        public async Task<IActionResult> Index()
        {
            var initFlagPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "zcymes2024.flag");
            if (System.IO.File.Exists(initFlagPath))
            {
                return Content("已初始化,无需重复");
            }

            var roleInit = await _roleService.InitRoleAsync();
            var userInit = await _userService.InitUserAsync();
            await System.IO.File.WriteAllTextAsync(initFlagPath, DateTime.Now.ToString());
            return Content($"初始化完成,{roleInit.Msg},{userInit.Msg}");
        }
    }
}
