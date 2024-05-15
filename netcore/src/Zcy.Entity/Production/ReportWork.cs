using System;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.Products;

namespace Zcy.Entity.Production
{
    /// <summary>
    /// 报工
    /// </summary>
    public class ReportWork : BaseEntity<long>
    {
        /// <summary>
        /// 报工
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="reportWorkDate">报工日期</param>
        /// <param name="productProcessId">工序Id</param>
        /// <param name="wordDuration">工作时长</param>
        public ReportWork(long employeeId,
            DateTime reportWorkDate,
            long productProcessId, decimal wordDuration)
        {
            EmployeeId = employeeId;
            ProductProcessId = productProcessId;
            WordDuration = wordDuration;
            ReportWorkDate= reportWorkDate;
        }

        /// <summary>
        /// 报工日期
        /// </summary>
        public DateTime ReportWorkDate { get; protected set; }

        /// <summary>
        /// 员工用户Id
        /// </summary>
        public long EmployeeId { get; protected set; }

        /// <summary>
        /// 产品工序Id
        /// </summary>
        public long ProductProcessId { get; protected set; }

        /// <summary>
        /// 工作时长
        /// </summary>
        /// <remarks>
        ///  数量或时长
        /// </remarks>
        public decimal WordDuration { get; protected set; }

        /// <summary>
        /// 应结算价格
        /// </summary>
        public decimal ReceivableSettlementPrice { get; protected set; }

        /// <summary>
        /// 实结算价格
        /// </summary>
        public decimal ActualSettlementPrice { get; protected set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

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

        /// <summary>
        /// 工艺计费类型 冗余
        /// </summary>
        public BillingTypeEnum BillingType { get; set; }
        #endregion

        /// <summary>
        /// 计算应收价格
        /// </summary>
        public void TotalReceivablePrice()
        {
            if (UnitPrice == default ||
                WordDuration == default)
            {
                throw new ArgumentException($"{nameof(ReportWork)} TotalReceivablePrice is error，wordDuration or unitPrice is null");
            }

            ReceivableSettlementPrice = (ProcessingPrice + UnitPrice) * WordDuration;
            ActualSettlementPrice = ReceivableSettlementPrice;
        }

        /// <summary>
        /// 设置工序信息
        /// </summary>
        /// <param name="processingPrice">工艺价格</param>
        /// <param name="unitPrice">工序价格</param>
        /// <param name="billingType">工艺计费类型</param>
        public void SetProductProcessInfo(decimal processingPrice, decimal unitPrice,
            BillingTypeEnum billingType)
        {
            ProcessingPrice = processingPrice;
            UnitPrice = unitPrice;
            BillingType = billingType;
        }
    }
}
