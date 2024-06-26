using Ganss.Excel;
using Zcy.BaseInterface;
using Zcy.BaseInterface.BaseModel;
using Zcy.Dto.Production;
using Zcy.Dto.Production.Exports;
using Zcy.Entity.Production;
using Zcy.ExcelNpoi;
using Zcy.IRepository.Production;
using Zcy.IService.Production;
using Zcy.Utility;

namespace Zcy.Service.Production
{
    /// <summary>
    /// 报工导入|导出 服务实现
    /// </summary>
    public class ReportWorkImportAndExportAppService : ZcyBaseService, IReportWorkImportAndExportAppService
    {
        private readonly IReportWorkRepository _reportWorkRepository;
        private readonly IZcyExcelExtension _zcyExcelExtension;

        public ReportWorkImportAndExportAppService(IReportWorkRepository reportWorkRepository,
            IZcyExcelExtension zcyExcelExtension)
        {
            _reportWorkRepository = reportWorkRepository;
            _zcyExcelExtension = zcyExcelExtension;
        }

        /// <summary>
        /// 导出员工报工
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ExportDayReportWorkAsync(QueryPageReportWorkInput input)
        {
            #region db查询
            var query = await _reportWorkRepository.GetQueryableAsync();
            query = query.CreateConditions(input);
            var timeRange = BaseTimeRangeInputExt.GetTimeRange(input);

            query = query.Where(a => a.ReportWorkDate >= timeRange.sTime &&
                                     a.ReportWorkDate <= timeRange.eTime);
            var dbReportWork = await _reportWorkRepository.ToListAsync(query);
            #endregion

            #region 组合数据
            //第一个表格 汇总表格
            //后面按名称每个Sheet就是一个员工
            var dayReportWorkDto = BaseMapper
                .Map<IReadOnlyList<ReportWork>, List<ExportDayReportWorkDto>>(dbReportWork);

            //第一个汇总表格
            var summaryResult = dayReportWorkDto
                .GroupBy(a => new
                {
                    a.EmployeeNickName,
                    a.BillingType
                })
                .Select(a => new ExportDayReportWorkWithSummaryDto()
                {
                    EmployeeNickName = a.Key.EmployeeNickName,
                    BillingTypeStr = a.Key.BillingType.GetDisplayName(),
                    WordDuration = a.Sum(b => b.WordDuration)
                })
                .OrderBy(a => a.EmployeeNickName)
                .ThenBy(a => a.BillingTypeStr)
                .ToList();

            //其他表数据
            var employeeNickNames = dayReportWorkDto
                .Where(a => string.IsNullOrEmpty(a.EmployeeNickName) == false)
                .Select(a => a.EmployeeNickName)
                .Distinct()
                .OrderBy(a => a)
                .ToList();
            var dayReportWorkResult = new Dictionary<string, List<ExportDayReportWorkDto>>();
            foreach (var nickName in employeeNickNames)
            {
                var currentReportWork = dayReportWorkDto
                    .Where(a => a.EmployeeNickName == nickName)
                    .OrderBy(a => a.ReportWorkDate)
                    .ThenBy(a => a.EmployeeNickName)
                    .ThenBy(a => a.ProductName)
                    .ThenBy(a => a.ProductCraftName)
                    .ToList();
                dayReportWorkResult.Add(nickName, currentReportWork);
            }
            #endregion

            #region 写入多个sheet
            var mapper = new ExcelMapper();
            await _zcyExcelExtension.ExportXlsxWithMultipleSheetsAddAsync(mapper,
                new ZcyMultipleSheetInput<ExportDayReportWorkWithSummaryDto>("汇总", summaryResult));

            var index = 0;
            await using var ms = new MemoryStream();
            foreach (var item in dayReportWorkResult)
            {
                index++;
                if (index == dayReportWorkResult.Count)
                {
                    await _zcyExcelExtension.ExportXlsxWithMultipleSheetsWriteAsync(mapper, ms,
                        new ZcyMultipleSheetInput<ExportDayReportWorkDto>($"{item.Key} 明细", item.Value));
                }
                else
                {
                    await _zcyExcelExtension.ExportXlsxWithMultipleSheetsAddAsync(mapper,
                        new ZcyMultipleSheetInput<ExportDayReportWorkDto>($"{item.Key} 明细", item.Value));
                }
            }

            return await ms.GetAllBytesAsync();
            #endregion
        }

        /// <summary>
        /// 导出产品报工
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ExportProductReportWorkAsync(QueryPageReportWorkInput input)
        {
            #region db查询
            var query = await _reportWorkRepository.GetQueryableAsync();
            query = query.CreateConditions(input);
            var timeRange = BaseTimeRangeInputExt.GetTimeRange(input);

            query = query.Where(a => a.ReportWorkDate >= timeRange.sTime &&
                                     a.ReportWorkDate <= timeRange.eTime);
            var dbReportWork = await _reportWorkRepository.ToListAsync(query);
            #endregion

            #region 组合数据
            //第一个表格 汇总表格
            //后面按名称每个Sheet就是一个产品
            var dayReportWorkDto = BaseMapper
                .Map<IReadOnlyList<ReportWork>, List<ExportReportWorkWithProductDto>>(dbReportWork);

            //第一个汇总表格
            var summaryResult = dayReportWorkDto
                .GroupBy(a => new
                {
                    a.ProductName,
                    a.ProductCraftName,
                })
                .Select(a => new ExportReportWorkWithProductSummaryDto()
                {
                    ProductName = a.Key.ProductName,
                    ProductCraftName = a.Key.ProductCraftName,
                    WordDuration = a.Sum(b => b.WordDuration)
                })
                .OrderBy(a => a.ProductName)
                .ThenBy(a => a.ProductCraftName)
                .ToList();

            //其他表数据
            var productNames = dayReportWorkDto
                .Select(a => a.ProductName)
                .Distinct()
                .OrderBy(a => a)
                .ToList();
            var productReportWorkResult = new Dictionary<string, List<ExportReportWorkWithProductDto>>();
            foreach (var productName in productNames)
            {
                if (string.IsNullOrEmpty(productName))
                {
                    continue;
                }

                var currentReportWork = dayReportWorkDto
                    .Where(a => a.ProductName == productName)
                    .OrderBy(a => a.ReportWorkDate)
                    .ThenBy(a => a.ProductName)
                    .ThenBy(a => a.ProductCraftName)
                    .ToList();
                productReportWorkResult.Add(productName, currentReportWork);
            }
            #endregion

            #region 写入多个sheet
            var mapper = new ExcelMapper();
            await _zcyExcelExtension.ExportXlsxWithMultipleSheetsAddAsync(mapper,
                new ZcyMultipleSheetInput<ExportReportWorkWithProductSummaryDto>("汇总", summaryResult));

            var index = 0;
            await using var ms = new MemoryStream();
            foreach (var item in productReportWorkResult)
            {
                index++;
                if (index == productReportWorkResult.Count)
                {
                    await _zcyExcelExtension.ExportXlsxWithMultipleSheetsWriteAsync(mapper, ms,
                        new ZcyMultipleSheetInput<ExportReportWorkWithProductDto>($"【{item.Key}】产品明细", item.Value));
                }
                else
                {
                    await _zcyExcelExtension.ExportXlsxWithMultipleSheetsAddAsync(mapper,
                        new ZcyMultipleSheetInput<ExportReportWorkWithProductDto>($"【{item.Key}】产品明细", item.Value));
                }
            }

            return await ms.GetAllBytesAsync();
            #endregion
        }
    }
}
