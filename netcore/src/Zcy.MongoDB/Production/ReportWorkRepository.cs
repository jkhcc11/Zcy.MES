using MongoDB.Driver;
using Zcy.Entity.Production;
using Zcy.Entity.Products;
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
        public async Task<bool> IsTodayExistReportWorkAsync(long companyId, DateTime reportWorkDate, long employeeId, long productProcessId)
        {
            return await DbCollection
                .Find(a => a.CompanyId == companyId &&
                         a.ReportWorkDate == reportWorkDate &&
                         a.EmployeeId == employeeId)
                .AnyAsync();
        }
    }
}
