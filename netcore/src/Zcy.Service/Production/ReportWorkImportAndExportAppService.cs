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
                    a.BillingType,
                    a.ProductName,
                    a.ProductCraftName,
                })
                .Select(a => new ExportDayReportWorkWithSummaryDto()
                {
                    EmployeeNickName = a.Key.EmployeeNickName,
                    ProductName = a.Key.ProductName,
                    ProductCraftName = a.Key.ProductCraftName,
                    BillingTypeStr = a.Key.BillingType.GetDisplayName(),
                    WordDuration = a.Sum(b => b.WordDuration)
                })
                .OrderBy(a => a.EmployeeNickName)
                .ThenBy(a => a.ProductName)
                .ThenBy(a => a.ProductCraftName)
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

        /// <summary>
        /// 导出员工报工（横向）
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ExportDayReportWorkWithHorizontalAsync(QueryPageReportWorkInput input)
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

            //其他表数据
            var employeeNickNames = dayReportWorkDto
                .Where(a => string.IsNullOrEmpty(a.EmployeeNickName) == false)
                .Select(a => a.EmployeeNickName)
                .Distinct()
                .OrderBy(a => a)
                .ToList();
            var dayReportWorkResult = new List<DynamicHeadersAndSheetsModel>();
            foreach (var nickName in employeeNickNames)
            {
                //当前员工数据
                var currentReportWork = dayReportWorkDto
                    .Where(a => a.EmployeeNickName == nickName)
                    .ToList();
                //行日期分组
                var rowsDate = currentReportWork
                    .GroupBy(a => new
                    {
                        a.ReportWorkDate,
                    })
                    .Select(a => a.Key)
                    .ToList();
                //表头
                var headerTitle = currentReportWork
                    .GroupBy(a => new
                    {
                        a.ProductName,
                        a.ProductCraftName
                    })
                    .Select(a => new
                    {
                        a.Key.ProductName,
                        a.Key.ProductCraftName,
                        SumWordDuration = a.Sum(b => b.WordDuration)
                    })
                    .ToList();

                //工作表数据
                var sheetRowData = new List<Dictionary<string, object>>();
                foreach (var dateItem in rowsDate)
                {
                    //当天日期数据
                    var currentDateData = new Dictionary<string, object>
                    {
                        { "日期\\产品工序", dateItem.ReportWorkDate }
                    };
                    foreach (var headerItem in headerTitle
                                 .OrderBy(a => a.ProductName).ThenBy(a => a.ProductCraftName))
                    {
                        var currentDateProductCraft = currentReportWork
                            .FirstOrDefault(a => a.ReportWorkDate == dateItem.ReportWorkDate &&
                                        a.ProductName == headerItem.ProductName &&
                                        a.ProductCraftName == headerItem.ProductCraftName);
                        if (currentDateProductCraft == null)
                        {
                            //当前无数据
                            continue;
                        }

                        currentDateData.Add(
                            $"{headerItem.ProductName}-{headerItem.ProductCraftName}",
                            currentDateProductCraft.WordDuration);
                    }

                    sheetRowData.Add(currentDateData);
                }

                var sumRow = new Dictionary<string, object>()
                {
                    { "日期\\产品工序", "合计" },
                };
                foreach (var temp in headerTitle
                             .OrderBy(a => a.ProductName).ThenBy(a => a.ProductCraftName))
                {
                    sumRow.Add(
                        $"{temp.ProductName}-{temp.ProductCraftName}",
                        temp.SumWordDuration);
                }

                sheetRowData.Add(sumRow);

                var dayReportWorkResultItem = new DynamicHeadersAndSheetsModel(nickName, sheetRowData);
                dayReportWorkResult.Add(dayReportWorkResultItem);
            }
            #endregion

            // 写入 Excel 文件到 MemoryStream
            var stream = await _zcyExcelExtension.ExportXlsxWithMultipleHeadersAndSheetsWriteAsync(dayReportWorkResult);
            using (stream)
            {
                return await stream.GetAllBytesAsync();
            }
        }

        /// <summary>
        /// 导出员工报工（日期横向）
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ExportDayReportWorkWithDateHorizontalAsync(QueryPageReportWorkInput input)
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

            //其他表数据
            var employeeNickNames = dayReportWorkDto
                .Where(a => string.IsNullOrEmpty(a.EmployeeNickName) == false)
                .Select(a => a.EmployeeNickName)
                .Distinct()
                .OrderBy(a => a)
                .ToList();
            var dayReportWorkResult = new List<DynamicHeadersAndSheetsModel>();
            foreach (var nickName in employeeNickNames)
            {
                //当前员工数据
                var currentReportWork = dayReportWorkDto
                    .Where(a => a.EmployeeNickName == nickName)
                    .ToList();
                //行工序分组
                var rowsDate = currentReportWork
                    .GroupBy(a => new
                    {
                        a.ProductName,
                        a.ProductCraftName,
                    })
                    .Select(a => new
                    {
                        a.Key.ProductName,
                        a.Key.ProductCraftName,
                    })
                    .OrderBy(a => a.ProductName)
                    .ThenBy(a => a.ProductCraftName)
                    .ToList();
                //日期表头
                var dateHeaderTitle = currentReportWork
                    .GroupBy(a => new
                    {
                        a.ReportWorkDate
                    })
                    .Select(a => a.Key.ReportWorkDate)
                    .OrderBy(a => a)
                    .ToList();

                //是否跨月
                var isTwoMonth = dateHeaderTitle
                    .Select(Convert.ToDateTime)
                    .Select(a => a.Month)
                    .Distinct()
                    .Count() > 1;

                //工作表数据
                var sheetRowData = new List<Dictionary<string, object>>();
                for (int i = 0; i < rowsDate.Count; i++)
                {
                    var rowItem = rowsDate[i];
                    var currentColData = new Dictionary<string, object>();
                    if (i == 0)
                    {
                        //第一行
                        currentColData.Add("员工名", $"{nickName}");
                    }

                    currentColData.Add("工艺\\日期", $"{rowItem.ProductName}-{rowItem.ProductCraftName}");

                    for (int index = 0; index < dateHeaderTitle.Count; index++)
                    {
                        var dateHeaderTitleItem = dateHeaderTitle[index];
                        var currentDateColData = currentReportWork
                            .FirstOrDefault(a => a.ReportWorkDate == dateHeaderTitleItem &&
                                                 a.ProductName == rowItem.ProductName &&
                                                 a.ProductCraftName == rowItem.ProductCraftName);
                        var showTitle = isTwoMonth ?
                            Convert.ToDateTime(dateHeaderTitleItem)
                                .ToString("MM-dd") :
                            Convert.ToDateTime(dateHeaderTitleItem)
                                .ToString("dd");
                        if (currentDateColData != null)
                        {
                            //当前当天有数据
                            if (currentDateColData is { ProductCraftName: "请假", WordDuration: 8 })
                            {
                                //请假特殊处理
                                currentColData.Add($"{showTitle}", "请假一天");
                            }
                            else
                            {
                                currentColData.Add($"{showTitle}", $"{currentDateColData.WordDuration}");
                            }
                        }
                        else
                        {
                            //日期没数据置空
                            currentColData.Add($"{showTitle}", string.Empty);
                        }

                        //最后一列
                        if (index == dateHeaderTitle.Count - 1)
                        {
                            currentColData.Add("合计",
                                currentReportWork
                                    .Where(a => a.ProductName == rowItem.ProductName &&
                                                a.ProductCraftName == rowItem.ProductCraftName)
                                    .Sum(a => a.WordDuration));
                            sheetRowData.Add(currentColData);
                        }
                    }
                }

                var dayReportWorkResultItem = new DynamicHeadersAndSheetsModel(nickName, sheetRowData);
                dayReportWorkResult.Add(dayReportWorkResultItem);
            }
            #endregion

            // 写入 Excel 文件到 MemoryStream
            var stream = await _zcyExcelExtension.ExportXlsxWithMultipleHeadersAndSheetsWriteAsync(dayReportWorkResult);
            using (stream)
            {
                return await stream.GetAllBytesAsync();
            }
        }
    }
}
