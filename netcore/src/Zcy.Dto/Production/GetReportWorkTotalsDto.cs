namespace Zcy.Dto.Production
{
    /// <summary>
    /// 获取报工汇总
    /// </summary>
    public class GetReportWorkTotalsDto
    {
        /// <summary>
        /// 汇总应结算价格
        /// </summary>
        public decimal TotalReceivableSettlementPrice { get; set; }

        /// <summary>
        /// 汇总实结算价格
        /// </summary>
        public decimal TotalActualSettlementPrice { get; set; }

        /// <summary>
        /// 计时总工时
        /// </summary>
        public decimal? TimingWordDuration { get; set; }

        /// <summary>
        /// 计件总数
        /// </summary>
        public decimal? CountingWordDuration { get; set; }
    }
}
