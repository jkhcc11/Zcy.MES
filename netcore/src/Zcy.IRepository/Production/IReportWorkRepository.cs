using System;
using System.Threading.Tasks;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.Production;

namespace Zcy.IRepository.Production
{
    /// <summary>
    /// 报工 仓储接口
    /// </summary>
    public interface IReportWorkRepository : IBaseRepository<ReportWork, long>
    {
        /// <summary>
        /// 员工当天是否存在报工
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="reportWorkDate">报工日期</param>
        /// <param name="employeeId">员工Id</param>
        /// <param name="productProcessId">产品工序Id</param>
        /// <returns></returns>
        Task<bool> IsTodayExistReportWorkAsync(long companyId, DateTime reportWorkDate,
            long employeeId, long productProcessId);

        /// <summary>
        /// 员工当天是否存在报工
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="reportWorkDate">报工日期</param>
        /// <param name="employeeId">员工Id</param>
        /// <param name="productProcessIds">产品工序列表</param>
        /// <returns></returns>
        Task<bool> IsTodayExistReportWorkAsync(long companyId, DateTime reportWorkDate,
            long employeeId, long[] productProcessIds);

        /// <summary>
        /// 是否存在有效报工
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="reportIds">报工记录</param>
        /// <returns></returns>
        Task<bool> IsExistValidReportWorkAsync(long companyId, long[] reportIds);
    }
}
