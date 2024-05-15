using System;

namespace Zcy.BaseInterface.Entities
{
    /// <summary>
    /// 基础Entity
    /// </summary>
    public abstract class BaseEntity<TKey> : IBaseTimeKey, ISoftDelete
    where TKey : struct
    {
        protected BaseEntity()
        {

        }

        protected BaseEntity(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// 主键
        /// </summary>
        public TKey Id { get; set; }

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

    /// <summary>
    /// 创建时间接口
    /// </summary>
    public interface IBaseTimeKey
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreatedTime { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        long? CreatedUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        long? ModifyUserId { get; set; }
    }

    /// <summary>
    /// 软删除标记
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeletedTime { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        long? DeletedUserId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDelete { get; set; }
    }

}
