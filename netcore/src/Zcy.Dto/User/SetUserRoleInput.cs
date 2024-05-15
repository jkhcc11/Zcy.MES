namespace Zcy.Dto.User
{
    /// <summary>
    /// 设置用户角色
    /// </summary>
    public class SetUserRoleInput
    {
        public SetUserRoleInput(long userId, List<long> roleIds)
        {
            UserId = userId;
            RoleIds = roleIds;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
       
        /// <summary>
        /// 角色Ids
        /// </summary>
        public List<long> RoleIds { get; set; }
    }
}
