using MongoDB.Driver;
using Zcy.Entity.Company;
using Zcy.Entity.Production;
using Zcy.IRepository.Production;

namespace Zcy.MongoDB.Production
{
    /// <summary>
    ///  报工 仓储实现
    /// </summary>
    public class ReportWorkRepository : BaseMongodbRepository<ReportWork, long>, IReportWorkRepository
    {
        public ReportWorkRepository(ZcyMongodbContext zcyDbContext) : base(zcyDbContext)
        {

        }

        /// <summary>
        /// 员工当天是否存在报工
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="reportWorkDate">报工日期</param>
        /// <param name="employeeId">员工Id</param>
        /// <param name="productProcessId">产品工序Id</param>
        /// <returns></returns>
        public async Task<bool> IsTodayExistReportWorkAsync(long companyId, DateTime reportWorkDate,
            long employeeId, long productProcessId)
        {
            return await DbCollection
                .Find(a => a.CompanyId == companyId &&
                         a.ReportWorkDate == reportWorkDate &&
                         a.EmployeeId == employeeId &&
                         a.ProductProcessId == productProcessId &&
                         a.IsDelete == false &&
                         (int)a.ReportWorkStatus <= PublicStatusEnum.Pending.GetHashCode())
                .AnyAsync();
        }

        /// <summary>
        /// 员工当天是否存在报工
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="reportWorkDate">报工日期</param>
        /// <param name="employeeId">员工Id</param>
        /// <param name="productProcessIds">产品工序列表</param>
        /// <returns></returns>
        public async Task<bool> IsTodayExistReportWorkAsync(long companyId, DateTime reportWorkDate,
            long employeeId, long[] productProcessIds)
        {
            return await DbCollection
                .Find(a => a.CompanyId == companyId &&
                           a.ReportWorkDate == reportWorkDate &&
                           a.EmployeeId == employeeId &&
                           productProcessIds.Contains(a.ProductProcessId) &&
                           a.IsDelete == false &&
                           (int)a.ReportWorkStatus <= PublicStatusEnum.Pending.GetHashCode())
                .AnyAsync();
        }

        /// <summary>
        /// 是否存在有效报工
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="reportIds">报工记录</param>
        /// <returns></returns>
        public async Task<bool> IsExistValidReportWorkAsync(long companyId, long[] reportIds)
        {
            return await DbCollection
                .Find(a => a.CompanyId == companyId &&
                           reportIds.Contains(a.Id) &&
                           a.IsDelete == false &&
                           (int)a.ReportWorkStatus <= PublicStatusEnum.Pending.GetHashCode())
                .AnyAsync();
        }
    }
}
