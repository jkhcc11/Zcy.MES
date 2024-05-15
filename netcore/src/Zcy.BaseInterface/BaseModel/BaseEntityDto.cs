using System;

namespace Zcy.BaseInterface.BaseModel
{
    public abstract class BaseEntityDto<TKey>
        where TKey : struct
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TKey Id { get; set; }
    }

    public abstract class BaseFullAuditEntityDto<TKey> : BaseEntityDto<TKey>
        where TKey : struct
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long? CreatedUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public long? ModifyUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletedTime { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        public long? DeletedUserId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
