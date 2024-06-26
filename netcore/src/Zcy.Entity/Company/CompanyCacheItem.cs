﻿namespace Zcy.Entity.Company
{
    /// <summary>
    /// 公司缓存Item
    /// </summary>
    public class CompanyCacheItem
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// 缩写
        /// </summary>
        /// <remarks>
        /// 一般用于订单号拼接
        /// </remarks>
        public string? ShortName { get; set; }

        /// <summary>
        /// 公司抬头
        /// </summary>
        public string? CompanyShowName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
