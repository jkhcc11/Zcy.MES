using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Service;
using Zcy.Dto.SysBaseInfo;

namespace Zcy.IService.SysBaseInfo
{
    /// <summary>
    /// 角色菜单 服务接口
    /// </summary>
    public interface ISystemRoleMenuService : IZcyBaseService
    {
        /// <summary>
        /// 获取角色已激活菜单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetRoleActivateMenuAsync(long roleId);

        /// <summary>
        /// 创建和更新角色菜单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateRoleMenuAsync(CreateAndUpdateRoleMenuInput input);

        /// <summary>
        /// 获取角色已激活菜单树
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<GetRoleActivateMenuTreeDto>?>> GetRoleActivateMenuTreeAsync(long[] roleIds);
    }
}
