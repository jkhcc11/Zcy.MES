using System;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.Products;

namespace Zcy.Entity.Production
{
    /// <summary>
    /// 报工
    /// </summary>
    public class ReportWork : BaseEntity<long>, IBaseCompany
    {
        /// <summary>
        /// 报工
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="employeeNickName">员工用户昵称</param>
        /// <param name="reportWorkDate">报工日期</param>
        /// <param name="productProcessId">工序Id</param>
        /// <param name="wordDuration">工作时长</param>
        public ReportWork(long employeeId, string employeeNickName,
            DateTime reportWorkDate,
            long productProcessId, decimal wordDuration)
        {
            EmployeeId = employeeId;
            ProductProcessId = productProcessId;
            WordDuration = wordDuration;
            ReportWorkDate = reportWorkDate;
            EmployeeNickName = employeeNickName;
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
        /// 员工用户昵称
        /// </summary>
        public string EmployeeNickName { get; protected set; }

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

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }

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
        /// 基础价格，如果有根据产品+工序的在产品选择工序后加
        /// </remarks>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 工艺计费类型 冗余
        /// </summary>
        public BillingTypeEnum BillingType { get; set; }

        /// <summary>
        /// 产品名 冗余
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// 产品工艺名称  冗余
        /// </summary>
        public string? ProductCraftName { get; set; }

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
        /// <param name="processingPrice">加工价</param>
        /// <param name="craftPrice">工艺价格</param>
        /// <param name="billingType">工艺计费类型</param>
        /// <param name="productName">产品名</param>
        /// <param name="productCraftName">产品工艺名</param>
        public void SetProductProcessInfo(decimal processingPrice, decimal craftPrice,
            BillingTypeEnum billingType, string productName, string productCraftName)
        {
            ProcessingPrice = processingPrice;
            UnitPrice = craftPrice;
            BillingType = billingType;
            ProductName = productName;
            ProductCraftName = productCraftName;
        }

        /// <summary>
        /// 设置实际价格
        /// </summary>
        public void SetActualSettlementPrice(decimal actualSettlementPrice)
        {
            ActualSettlementPrice = actualSettlementPrice;
        }
    }
}
