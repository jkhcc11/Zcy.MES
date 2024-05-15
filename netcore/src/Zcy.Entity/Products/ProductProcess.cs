﻿using Zcy.BaseInterface.Entities;

namespace Zcy.Entity.Products
{
    /// <summary>
    /// 产品工序
    /// </summary>
    public class ProductProcess : BaseEntity<long>, IBaseCompany
    {
        /// <summary>
        /// 产品工序
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <param name="craftId">工艺Id</param>
        public ProductProcess(long productId, long craftId)
        {
            ProductId = productId;
            CraftId = craftId;
        }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工艺Id
        /// </summary>
        public long CraftId { get; set; }

        /// <summary>
        /// 加工价
        /// </summary>
        /// <remarks>
        /// 计算工资使用
        /// </remarks>
        public decimal ProcessingPrice { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }
    }
}
