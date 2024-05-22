﻿using Zcy.Entity.Company;
using Zcy.Entity.Products;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 创建产品工艺
    /// </summary>
    public class CreateProductCraftInput
    {
        public CreateProductCraftInput(string craftName)
        {
            CraftName = craftName;
        }

        /// <summary>
        /// 工艺名
        /// </summary>
        public string CraftName { get; set; }

        /// <summary>
        /// 工艺类型
        /// </summary>
        /// <remarks>
        /// 独立工艺: ■一般为计时 不用配置产品线使用。比如捡货计时|收货计时 <br/>
        /// 加工工艺：■配合产品使用
        /// </remarks>
        public CraftTypeEnum CraftType { get; set; }

        /// <summary>
        /// 计费类型
        /// </summary>
        public BillingTypeEnum BillingType { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        /// <remarks>
        /// 工序基础价格，如果有根据产品+工序的在产品选择工序后加
        /// </remarks>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
