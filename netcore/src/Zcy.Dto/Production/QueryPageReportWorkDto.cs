using Zcy.BaseInterface.Entities;

namespace Zcy.Dto.Production
{
    /// <summary>
    /// 分页查询报工
    /// </summary>
    public class QueryPageReportWorkDto : QueryPageReportWorkForAdminDto, IBaseCompanyDto
    {
        /// <summary>
        /// 应结算价格
        /// </summary>
        public decimal ReceivableSettlementPrice { get; set; }

        /// <summary>
        /// 实结算价格
        /// </summary>
        public decimal ActualSettlementPrice { get; set; }

        #region 冗余
        /// <summary>
        /// 加工价 冗余
        /// </summary>
        /// <remarks>
        /// 计算工资使用
        /// </remarks>
        public decimal ProcessingPrice { get; set; }

        /// <summary>
        /// 工艺价格 冗余
        /// </summary>
        /// <remarks>
        /// 工序基础价格，如果有根据产品+工序的在产品选择工序后加
        /// </remarks>
        public decimal UnitPrice { get; set; }
        #endregion

    }
}
