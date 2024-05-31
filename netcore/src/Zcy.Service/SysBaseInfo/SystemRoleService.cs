using Microsoft.Extensions.Caching.Distributed;
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
        private const string AllRoleCacheKey = "AllRoleCacheKey";
        private readonly IBaseRepository<SystemRole, long> _roleRepository;
        private readonly IDistributedCache _distributedCache;

        public SystemRoleService(IBaseRepository<SystemRole, long> roleRepository,
            IDistributedCache distributedCache)
        {
            _roleRepository = roleRepository;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// 分页查询角色
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageRoleDto>>> QueryPageRoleAsync(QueryPageRoleInput input)
        {
            var query = await _roleRepository.GetQueryableAsync();
            query = query.CreateConditions(input);
            var result = await BaseQueryPageEntityAsync<SystemRole, QueryPageRoleDto>(_roleRepository,
                query,
                input);

            return result;
        }

        /// <summary>
        /// 创建|修改角色
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateAndUpdateRoleAsync(CreateAndUpdateRoleInput input)
        {
            await ClearAllRoleCacheAsync();
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

            await ClearAllRoleCacheAsync();
            return KdyResult.Success();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<IList<GetAllRoleDto>> GetAllRoleAsync()
        {
            var dbList = await _roleRepository.ToListAsync();
            var result = BaseMapper.Map<IReadOnlyList<SystemRole>, IList<GetAllRoleDto>>(dbList);
            return result;
        }

        /// <summary>
        /// 获取角色列表(缓存)
        /// </summary>
        /// <returns></returns>
        public async Task<IList<GetAllRoleDto>?> GetAllRoleCacheAsync()
        {
            var result = await _distributedCache.GetOrCreateAsync(AllRoleCacheKey,
                async () => await GetAllRoleAsync(), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });

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
        /// 清空缓存
        /// </summary>
        /// <returns></returns>
        private async Task ClearAllRoleCacheAsync()
        {
            await _distributedCache.RemoveAsync(AllRoleCacheKey);
        }

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
