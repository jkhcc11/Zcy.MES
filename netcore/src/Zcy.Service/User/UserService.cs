using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Dto.User;
using Zcy.Entity.SysBaseInfo;
using Zcy.Entity.User;
using Zcy.IRepository.User;
using Zcy.IService.User;

namespace Zcy.Service.User
{
    /// <summary>
    /// 用户 服务实现
    /// </summary>
    public class UserService : ZcyBaseService, IUserService
    {
        private readonly ISystemUserRepository _userRepository;
        private readonly IBaseRepository<SystemRole, long> _roleRepository;
        private readonly IBaseRepository<SystemUserRole, long> _userRoleRepository;

        public UserService(ISystemUserRepository userRepository, IBaseRepository<SystemRole, long> roleRepository,
            IBaseRepository<SystemUserRole, long> userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<QueryPageDto<QueryPageUserDto>>> QueryUserAsync(QueryPageUserInput input)
        {
            var query = await _userRepository.GetQueryableAsync();
            if (string.IsNullOrEmpty(input.KeyWord) == false)
            {
                query = query.Where(a => a.UserName.Contains(input.KeyWord) ||
                                         a.UserNick.Contains(input.KeyWord));
            }

            if (LoginUserInfo.IsSuperAdmin == false)
            {
                query = query.Where(a => a.CompanyId == LoginUserInfo.CompanyId);
            }

            var dbResult = await _userRepository.QueryPageListAsync(query, input.Page, input.PageSize);
            if (dbResult.Items.Any() == false)
            {
                return KdyResult.Success(new QueryPageDto<QueryPageUserDto>());
            }

            //用户角色
            var allRole = await _roleRepository.GetAllListAsync();
            var userRoleQuery = await _userRoleRepository.GetQueryableAsync();
            var userIds = dbResult.Items.Select(a => a.Id).ToArray();
            userRoleQuery = userRoleQuery.Where(a => userIds.Contains(a.UserId));
            var userRoles = await _userRoleRepository.ToListAsync(userRoleQuery);

            var result = new QueryPageDto<QueryPageUserDto>()
            {
                Items = BaseMapper.Map<IReadOnlyList<SystemUser>, IReadOnlyList<QueryPageUserDto>>(dbResult.Items),
                Total = dbResult.Total
            };
            foreach (var item in result.Items)
            {
                var currentRoleIds = userRoles
                    .Where(a => a.UserId == item.Id)
                    .Select(a => a.RoleId)
                    .ToArray();
                if (currentRoleIds.Any() == false)
                {
                    continue;
                }

                item.Roles = allRole
                    .Where(a => currentRoleIds.Contains(a.Id))
                    .Select(a => a.RoleShowName)
                    .ToList();
                item.RoleIds = currentRoleIds;
            }

            return KdyResult.Success(result);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<UserLoginDto>> UserLoginAsync(UserLoginInput input)
        {
            var userInfo = await _userRepository.FindUserByUserNameAsync(input.UserName);
            if (userInfo == null)
            {
                return KdyResult.Error<UserLoginDto>(KdyResultCode.Error, "用户名或密码错误,请检测用户名或密码01");
            }

            if (userInfo.IsEnableLogin == false)
            {
                return KdyResult.Error<UserLoginDto>(KdyResultCode.Error, "该账号未启用登录，请联系管理者");
            }

            if (userInfo.PwdIsOk(input.UserPwd) == false)
            {
                return KdyResult.Error<UserLoginDto>(KdyResultCode.Error, "用户名或密码错误,请检测用户名或密码02");
            }

            #region 生成Token
            var userRole = await _userRepository.GetUserRoleAsync(userInfo.Id);
            var claims = new List<Claim>()
            {
                new(JwtClaimTypes.Name, userInfo.UserName),
                new(JwtClaimTypes.NickName, userInfo.UserNick),
                new(AuthorizationConst.ZcyClaimTypes.CompanyId, userInfo.CompanyId.ToString()),
                new (JwtClaimTypes.Subject, userInfo.Id.ToString()),
            };

            if (string.IsNullOrEmpty(userInfo.UserNo) == false)
            {
                claims.Add(new Claim(AuthorizationConst.ZcyClaimTypes.UserNo, userInfo.UserNo));
            }

            foreach (var systemRole in userRole)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, systemRole.RoleName));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            #endregion

            return KdyResult.Success(new UserLoginDto(userInfo.CompanyId, userInfo.UserNick, encodedToken)
            {
                BaseSettlement = userInfo.BaseSettlement,
                UserNo = userInfo.UserNo
            });
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> CreateUserAsync(CreateUserInput input)
        {
            if (await _userRepository.AnyAsync(a => a.UserName == input.UserName))
            {
                return KdyResult.Error<UserLoginDto>(KdyResultCode.Error, "用户名已存在，创建失败");
            }

            var dbEntity = new SystemUser(input.UserName, input.UserNick)
            {
                BaseSettlement = input.BaseSettlement,
                CompanyId = input.CompanyId,
                UserPhone = input.UserPhone,
                UserNo = input.UserNo
            };

            if (input.IsEnableLogin)
            {
                dbEntity.EnableLogin();
            }

            await _userRepository.CreateAsync(dbEntity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyUserInfoAsync(ModifyUserInfoInput input)
        {
            var dbEntity = await _userRepository.FirstOrDefaultAsync(input.UserId);
            if (dbEntity == null)
            {
                return KdyResult.Error<UserLoginDto>(KdyResultCode.Error, "非法访问，用户Id不存在");
            }

            dbEntity.ModifyNick(input.UserNick);
            dbEntity.UserPhone = input.UserPhone;
            dbEntity.UserNo = input.UserNo;
            dbEntity.BaseSettlement = input.BaseSettlement;
            await _userRepository.UpdateAsync(dbEntity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 用户密码修改
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> ModifyUserPwdAsync(ModifyUserPwdInput input)
        {
            var dbEntity = await _userRepository.GetEntityByIdAsync(LoginUserInfo.GetUserId());
            if (dbEntity.PwdIsOk(input.OldPwd) == false)
            {
                return KdyResult.Error<UserLoginDto>(KdyResultCode.Error, "旧密码错误");
            }

            dbEntity.ModifyPwd(input.NewPwd1);
            await _userRepository.UpdateAsync(dbEntity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 启用/禁用 用户登录
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> EnableUserLoginAsync(long userId)
        {
            var dbEntity = await _userRepository.FirstOrDefaultAsync(userId);
            if (dbEntity == null)
            {
                return KdyResult.Error<UserLoginDto>(KdyResultCode.Error, "非法访问，用户Id不存在");
            }

            if (dbEntity.IsEnableLogin)
            {
                //todo:禁用登录时 token问题
                dbEntity.DisableLogin();
            }
            else
            {
                dbEntity.EnableLogin();
            }

            await _userRepository.UpdateAsync(dbEntity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> DeleteUserAsync(long userId)
        {
            var dbEntity = await _userRepository.FirstOrDefaultAsync(userId);
            if (dbEntity == null)
            {
                return KdyResult.Error<UserLoginDto>(KdyResultCode.Error, "非法访问，用户Id不存在");
            }

            await _userRepository.DeleteAsync(dbEntity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <remarks>
        /// 重置用户密码为123456
        /// </remarks>
        /// <returns></returns>
        public async Task<KdyResult> ResetUserPwdAsync(long userId)
        {
            var dbEntity = await _userRepository.FirstOrDefaultAsync(userId);
            if (dbEntity == null)
            {
                return KdyResult.Error<UserLoginDto>(KdyResultCode.Error, "非法访问，用户Id不存在");
            }

            dbEntity.SetDefaultPwd();
            await _userRepository.UpdateAsync(dbEntity);
            return KdyResult.Success();
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> SetUserRoleAsync(SetUserRoleInput input)
        {
            //删除旧的
            var userRolesQuery = await _userRoleRepository.GetQueryableAsync();
            userRolesQuery = userRolesQuery.Where(a => a.UserId == input.UserId);
            var userRoles = await _userRoleRepository.ToListAsync(userRolesQuery);
            await _userRoleRepository.DeleteAsync(userRoles);

            //新增新的
            //todo:更换角色时 token问题
            var entities = input.RoleIds.Select(a => new SystemUserRole(input.UserId, a)).ToList();
            await _userRoleRepository.CreateAsync(entities);

            return KdyResult.Success();
        }

        /// <summary>
        /// 初始化用户
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult> InitUserAsync()
        {
            //角色
            var roleEntity =
                await _roleRepository.FirstOrDefaultAsync(a =>
                    a.RoleName == AuthorizationConst.NormalRoleName.SuperAdmin);
            if (roleEntity == null)
            {
                return KdyResult.Error(KdyResultCode.Error, "无角色");
            }

            //用户
            var userName = "admin";
            if (await _userRepository.AnyAsync(a => a.UserName == userName))
            {
                return KdyResult.Error(KdyResultCode.Error, "用户已存在");
            }

            var entity = new SystemUser(userName, "超管");
            entity.EnableLogin();
            await _userRepository.CreateAsync(entity);

            //用户角色
            var userRoleEntity = new SystemUserRole(entity.Id, roleEntity.Id);
            await _userRoleRepository.CreateAsync(userRoleEntity);

            return KdyResult.Success();
        }

        /// <summary>
        /// 获取当前公司所有员工列表
        /// </summary>
        /// <returns></returns>
        public async Task<KdyResult<List<GetCurrentCompanyAllEmployeeDto>>> GetCurrentCompanyAllEmployeeAsync()
        {
            var companyId = LoginUserInfo.CompanyId;
            var userId = LoginUserInfo.GetUserId();
            var allEmployee = await _userRepository.GetCompanyAllEmployeeAsync(companyId, userId);
            var result = BaseMapper.Map<List<SystemUser>, List<GetCurrentCompanyAllEmployeeDto>>(allEmployee);
            return KdyResult.Success(result);
        }
    }
}
