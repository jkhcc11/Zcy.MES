using Microsoft.Extensions.DependencyInjection;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.SysBaseInfo;
using Zcy.Entity.SysBaseInfo;
using Zcy.IService.SysBaseInfo;

namespace Zcy.Service.SysBaseInfo
{
    /// <summary>
    /// 角色 服务实现
    /// </summary>
    public class SystemRoleService : ZcyBaseService, ISystemRoleService
    {
        private readonly IBaseRepository<SystemRole, long> _roleRepository;

        public SystemRoleService(IBaseRepository<SystemRole, long> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 分页查询角色
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageRoleDto>>> QueryPageRoleAsync(QueryPageRoleInput input)
        {
            var query = await _roleRepository.GetQueryableAsync();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                query = query.Where(a => a.RoleName.Contains(input.KeyWord) ||
                                         a.RoleShowName.Contains(input.KeyWord));
            }

            var result = await _roleRepository.QueryPageListAsync<QueryPageRoleDto>(query, input.Page, input.PageSize);
            return KdyResult.Success(result);
        }

        /// <summary>
        /// 创建|修改角色
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateRoleAsync(CreateAndUpdateRoleInput input)
        {
            if (input.Id.HasValue)
            {
                return await ModifyRoleAsync(input);
            }

            return await CreateRoleAsync(input);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> DeleteRoleAsync(long roleId)
        {
            var entity = await _roleRepository.GetEntityByIdAsync(roleId);
            await _roleRepository.DeleteAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<IList<GetAllRoleDto>> GetAllRoleAsync()
        {
            var dbList = await _roleRepository.GetAllListAsync();
            var result = BaseMapper.Map<IReadOnlyList<SystemRole>, IList<GetAllRoleDto>>(dbList);
            return result;
        }

        /// <summary>
        /// 初始化角色
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> InitRoleAsync()
        {
            var rootEntity = new SystemRole(AuthorizationConst.NormalRoleName.SuperAdmin, "超管")
            {
                Remark = "超管"
            };
            await _roleRepository.CreateAsync(rootEntity);
            return KdyResult.Success();
        }

        #region 私有
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> CreateRoleAsync(CreateAndUpdateRoleInput input)
        {
            if (await _roleRepository.AnyAsync(a => a.RoleName == input.RoleName))
            {
                return KdyResult.Error(KdyResultCode.Error, "角色已存在，请修改");
            }

            var entity = new SystemRole(input.RoleName, input.RoleShowName)
            {
                IsDefault = input.IsDefault,
                Remark = input.Remark
            };
            await _roleRepository.CreateAsync(entity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        private async Task<KdyResult> ModifyRoleAsync(CreateAndUpdateRoleInput input)
        {
            if (await _roleRepository.AnyAsync(a => a.RoleName == input.RoleName &&
                                                    a.Id != input.Id))
            {
                return KdyResult.Error(KdyResultCode.Error, "角色已存在，请修改");
            }

            var entity = await _roleRepository.GetEntityByIdAsync(input.Id.Value);
            entity.Remark = input.Remark;
            entity.RoleName = input.RoleName;
            entity.IsDefault = input.IsDefault;
            entity.RoleShowName = input.RoleShowName;
            await _roleRepository.UpdateAsync(entity);
            return KdyResult.Success();
        }
        #endregion
    }
}
