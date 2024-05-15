using System;
using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.SysBaseInfo
{
    /// <summary>
    /// 订单号生成配置
    /// </summary>
    /// <remarks>
    ///  todo:同公司同业务类型 并发生成需要特殊处理
    /// </remarks>
    public class GeneralOrderNoConfig : BaseEntity<long>
    {
        /// <summary>
        /// 订单号生成配置
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="orderNoPrefix">订单号前缀</param>
        /// <param name="isDayReset">是否每天重置序号</param>
        public GeneralOrderNoConfig(long companyId, string orderNoPrefix, bool isDayReset)
        {
            CompanyId = companyId;
            OrderNoPrefix = orderNoPrefix;
            LastNoGeneralDate = DateTime.Today;
            IsDayReset = isDayReset;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; protected set; }

        /// <summary>
        /// 订单号前缀
        /// </summary>
        public string OrderNoPrefix { get; protected set; }

        /// <summary>
        /// 单号序号
        /// </summary>
        public int NoIndex { get; protected set; }

        /// <summary>
        /// 最后一次生成订单号的日期
        /// </summary>
        public DateTime LastNoGeneralDate { get; protected set; }

        /// <summary>
        /// 是否每天重置序号
        /// </summary>
        /// <remarks>
        /// 如果配置后，当前日期和订单不一致则重置序号
        /// </remarks>
        public bool IsDayReset { get; protected set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 获取序号
        /// </summary>
        public int GetNoIndex()
        {
            if (IsDayReset == false)
            {
                //非重置
                NoIndex++;
                LastNoGeneralDate = DateTime.Today;
                return NoIndex;
            }

            if (LastNoGeneralDate == DateTime.Today)
            {
                //重置且是当天
                NoIndex++;
                return NoIndex;
            }

            //非当天重置
            NoIndex = 1;
            LastNoGeneralDate = DateTime.Today;
            return NoIndex;
        }
    }
}
