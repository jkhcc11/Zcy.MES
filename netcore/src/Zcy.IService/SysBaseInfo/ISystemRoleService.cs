using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Service;
using Zcy.Dto.SysBaseInfo;

namespace Zcy.IService.SysBaseInfo
{
    /// <summary>
    /// 角色 服务接口
    /// </summary>
    public interface ISystemRoleService : IZcyBaseService
    {
        /// <summary>
        /// 分页查询角色
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageRoleDto>>> QueryPageRoleAsync(QueryPageRoleInput input);

        /// <summary>
        /// 创建|修改角色
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateRoleAsync(CreateAndUpdateRoleInput input);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> DeleteRoleAsync(long roleId);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        Task<IList<GetAllRoleDto>> GetAllRoleAsync();

        /// <summary>
        /// 获取角色列表(缓存)
        /// </summary>
        /// <returns></returns>
        Task<IList<GetAllRoleDto>?> GetAllRoleCacheAsync();

        /// <summary>
        /// 初始化角色
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> InitRoleAsync();
    }
}
