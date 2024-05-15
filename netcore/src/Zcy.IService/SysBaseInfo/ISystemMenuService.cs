using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Service;
using Zcy.Dto.SysBaseInfo;

namespace Zcy.IService.SysBaseInfo
{
    /// <summary>
    /// 菜单 服务接口
    /// </summary>
    public interface ISystemMenuService : IZcyBaseService
    {
        /// <summary>
        /// 查询菜单列表
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<QueryPageDto<QueryPageMenuDto>>> QueryPageMenuAsync(QueryPageMenuInput input);

        /// <summary>
        /// 获取所有菜单树
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<List<GetAllMenuTreeDto>>> GetAllMenuTreeAsync();

        /// <summary>
        /// 创建和更新菜单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> CreateAndUpdateMenuAsync(CreateAndUpdateMenuInput input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        Task<KdyResult> BatchDeleteAsync(BatchOperationsInput input);

        /// <summary>
        /// 获取所有一级菜单
        /// </summary>
        /// <returns></returns>
        Task<KdyResult<IList<GetRoleActivateMenuDto>>> GetAllOneMenuAsync();
    }
}
