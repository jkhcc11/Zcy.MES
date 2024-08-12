using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ganss.Excel;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Zcy.Utility;

namespace Zcy.ExcelNpoi
{
    /// <summary>
    /// ExcelMapper扩展
    /// </summary>
    /// <remarks>
    /// 更多用法：https://github.com/mganss/ExcelMapper
    /// </remarks>
    public class ZcyExcelExtension : IZcyExcelExtension
    {
        /// <summary>
        /// Excel强类型解析
        /// </summary>
        /// <typeparam name="TData">输出类型</typeparam>
        /// <param name="filePath">文件路径（物理路径）</param>
        /// <param name="isHasHeaderRow">是否有头部行</param>
        /// <param name="skipBlankRows">是否跳过空白行</param>
        /// <param name="minRowNumber">最小从第几行开始 默认从头开始</param>
        /// <param name="maxRowNumber">最大读取行</param>
        /// <param name="sheetIndex">工作簿序号（默认为第一个,从0开始）</param>
        /// <returns></returns>
        public async Task<IEnumerable<TData>> ParseXlsxAsync<TData>(string filePath, bool isHasHeaderRow = true,
            bool skipBlankRows = true, int minRowNumber = 0, int maxRowNumber = int.MaxValue, int sheetIndex = 0)
            where TData : class, new()
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var mapper = new ExcelMapper()
            {
                HeaderRow = isHasHeaderRow,
                SkipBlankRows = skipBlankRows,
                MaxRowNumber = maxRowNumber,
                MinRowNumber = minRowNumber
            };
            var temp = await mapper.FetchAsync<TData>(filePath, sheetIndex);
            return temp;
        }

        public async Task<IEnumerable<TData>> ParseXlsxAsync<TData>(Stream fileStream, bool isHasHeaderRow = true,
            bool skipBlankRows = true, int minRowNumber = 0, int maxRowNumber = int.MaxValue, int sheetIndex = 0)
            where TData : class, new()
        {
            if (fileStream == null ||
                fileStream.Length <= 0)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            var mapper = new ExcelMapper()
            {
                HeaderRow = isHasHeaderRow,
                SkipBlankRows = skipBlankRows,
                MaxRowNumber = maxRowNumber,
                MinRowNumber = minRowNumber
            };
            var temp = await mapper.FetchAsync<TData>(fileStream, sheetIndex);
            return temp;
        }

        public async Task<IEnumerable<TData>> ParseXlsxAsync<TData>(Stream fileStream, string sheetName,
            bool isHasHeaderRow = true, bool skipBlankRows = true, int minRowNumber = 0,
            int maxRowNumber = int.MaxValue) where TData : class, new()
        {
            if (fileStream == null ||
                fileStream.Length <= 0)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            var mapper = new ExcelMapper()
            {
                HeaderRow = isHasHeaderRow,
                SkipBlankRows = skipBlankRows,
                MaxRowNumber = maxRowNumber,
                MinRowNumber = minRowNumber
            };
            var temp = await mapper.FetchAsync<TData>(fileStream, sheetName);
            return temp;
        }

        /// <summary>
        /// Excel弱类型解析
        /// </summary>
        /// <param name="filePath">文件路径（物理路径）</param>
        /// <param name="isHasHeaderRow">是否有头部行</param>
        /// <param name="skipBlankRows">是否跳过空白行</param>
        /// <param name="minRowNumber">最小从第几行开始 默认从头开始</param>
        /// <param name="maxRowNumber">最大读取行</param>
        /// <param name="sheetIndex">工作簿序号（默认为第一个,从0开始）</param>
        /// <returns></returns>
        public async Task<IEnumerable<dynamic>> ParseXlsxWithDynamicAsync(string filePath,
            bool isHasHeaderRow = true, bool skipBlankRows = true,
            int minRowNumber = 0, int maxRowNumber = int.MaxValue, int sheetIndex = 0)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var mapper = new ExcelMapper()
            {
                HeaderRow = isHasHeaderRow,
                SkipBlankRows = skipBlankRows,
                MaxRowNumber = maxRowNumber,
                MinRowNumber = minRowNumber
            };
            var temp = await mapper.FetchAsync(filePath, sheetIndex);
            return temp;
        }

        public async Task<IEnumerable<dynamic>> ParseXlsxWithDynamicAsync(Stream fileStream,
            bool isHasHeaderRow = true, bool skipBlankRows = true,
            int minRowNumber = 0, int maxRowNumber = int.MaxValue, int sheetIndex = 0)
        {
            if (fileStream == null ||
                fileStream.Length <= 0)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            var mapper = new ExcelMapper()
            {
                HeaderRow = isHasHeaderRow,
                SkipBlankRows = skipBlankRows,
                MaxRowNumber = maxRowNumber,
                MinRowNumber = minRowNumber
            };
            var temp = await mapper.FetchAsync(fileStream, sheetIndex);
            return temp;
        }

        public async Task<IEnumerable<dynamic>> ParseXlsxWithDynamicAsync(Stream fileStream,
            string sheetName, bool isHasHeaderRow = true, bool skipBlankRows = true,
            int minRowNumber = 0, int maxRowNumber = int.MaxValue)
        {
            if (fileStream == null ||
                fileStream.Length <= 0)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }

            var mapper = new ExcelMapper()
            {
                HeaderRow = isHasHeaderRow,
                SkipBlankRows = skipBlankRows,
                MaxRowNumber = maxRowNumber,
                MinRowNumber = minRowNumber
            };
            var temp = await mapper.FetchAsync(fileStream, sheetName);
            return temp;
        }

        /// <summary>
        /// Excel 特殊解析 适用于表头是A列
        /// </summary>
        /// <typeparam name="TData">输出类型</typeparam>
        /// <param name="filePath">文件路径（物理路径）</param>
        /// <param name="isHasHeaderRow">是否有头部行</param>
        /// <param name="skipBlankRows">是否跳过空白行</param>
        /// <param name="minRowNumber">最小从第几行开始 默认从头开始</param>
        /// <param name="maxRowNumber">最大读取行</param>
        /// <param name="sheetIndex">工作簿序号（默认为第一个,从0开始）</param>
        /// <returns></returns>
        public async Task<TData> ParseSpecialXlsxAsync<TData>(string filePath, bool isHasHeaderRow = true,
            bool skipBlankRows = true, int minRowNumber = 0,
            int maxRowNumber = int.MaxValue, int sheetIndex = 0)
            where TData : class, new()
        {
            var dynamicDataList = await ParseXlsxWithDynamicAsync(filePath, isHasHeaderRow, skipBlankRows, minRowNumber,
                maxRowNumber, sheetIndex);

            return ParseSpecialXlsx<TData>(dynamicDataList);
        }

        public async Task<TData> ParseSpecialXlsxAsync<TData>(Stream fileStream, bool isHasHeaderRow = true,
            bool skipBlankRows = true, int minRowNumber = 0,
            int maxRowNumber = Int32.MaxValue, int sheetIndex = 0)
            where TData : class, new()
        {
            var dynamicDataList = await ParseXlsxWithDynamicAsync(fileStream, isHasHeaderRow, skipBlankRows, minRowNumber,
                maxRowNumber, sheetIndex);

            return ParseSpecialXlsx<TData>(dynamicDataList);
        }

        #region 导出
        /// <summary>
        /// Excel导出
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="absoluteFilePath">保存文件绝对路径</param>
        /// <param name="objects">导出得数据</param>
        /// <param name="sheetName">sheet名 默认：sheet1</param>
        /// <param name="valueConverter">值转换</param>
        /// <returns></returns>
        public async Task ExportXlsxAsync<TData>(string absoluteFilePath, IEnumerable<TData> objects,
            string sheetName = "sheet1", Func<string, object, object>? valueConverter = null)
            where TData : class, new()
        {
            var excelMapper = new ExcelMapper();
            await excelMapper.SaveAsync(absoluteFilePath, objects, sheetName, true, valueConverter);
        }

        public async Task ExportXlsxAsync<TData>(Stream fileStream, IEnumerable<TData> objects,
            string sheetName = "sheet1", Func<string, object, object>? valueConverter = null)
            where TData : class, new()
        {
            var excelMapper = new ExcelMapper();
            await excelMapper.SaveAsync(fileStream, objects, sheetName, true, valueConverter);
        }

        /// <summary>
        /// Excel模板导出
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="tplAbsoluteFilePath">默认文件绝对路径</param>
        /// <param name="absoluteFilePath">保存文件绝对路径</param>
        /// <param name="objects">导出得数据</param>
        /// <param name="sheetName">sheet必须跟模板sheet一致</param>
        /// <param name="valueConverter">值转换</param>
        /// <returns></returns>
        public async Task ExportXlsxWithTplAsync<TData>(string tplAbsoluteFilePath, string absoluteFilePath,
            IEnumerable<TData> objects, string sheetName, Func<string, object, object>? valueConverter = null)
            where TData : class, new()
        {
            var excelMapper = new ExcelMapper(tplAbsoluteFilePath);
            await excelMapper.SaveAsync(absoluteFilePath, objects, sheetName, true, valueConverter);
        }

        /// <summary>
        /// Excel 模板导出(文件流)
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="tplAbsoluteFilePath">模板文件绝对路径</param>
        /// <param name="fileStream">保存文件流</param>
        /// <param name="objects">导出得数据</param>
        /// <param name="sheetName">sheet必须跟模板sheet一致</param>
        /// <param name="valueConverter">值转换</param>
        /// <returns></returns>
        public async Task ExportXlsxWithTplAsync<TData>(string tplAbsoluteFilePath, Stream fileStream,
            IEnumerable<TData> objects, string sheetName,
            Func<string, object, object>? valueConverter = null)
            where TData : class, new()
        {
            var excelMapper = new ExcelMapper(tplAbsoluteFilePath);
            await excelMapper.SaveAsync(fileStream, objects, sheetName, true, valueConverter);
        }

        /// <summary>
        /// Excel 自定义导出
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="excelMapper">导出配置</param>
        /// <param name="absoluteFilePath">保存文件绝对路径</param>
        /// <param name="objects">导出得数据</param>
        /// <param name="sheetName">sheet名 默认：sheet1</param>
        /// <param name="valueConverter">值转换</param>
        /// <returns></returns>
        public async Task ExportXlsxWithCustomAsync<TData>(ExcelMapper excelMapper, string absoluteFilePath,
            IEnumerable<TData> objects, string sheetName = "sheet1",
            Func<string, object, object>? valueConverter = null)
            where TData : class, new()
        {
            await excelMapper.SaveAsync(absoluteFilePath, objects, sheetName, true, valueConverter);
        }

        /// <summary>
        /// Excel 自定义导出(文件流)
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="excelMapper">导出配置</param>
        /// <param name="fileStream">保存文件流</param>
        /// <param name="objects">导出得数据</param>
        /// <param name="sheetName">sheet名 默认：sheet1</param>
        /// <param name="valueConverter">值转换</param>
        /// <returns></returns>
        public async Task ExportXlsxWithCustomAsync<TData>(ExcelMapper excelMapper, Stream fileStream,
            IEnumerable<TData> objects, string sheetName,
            Func<string, object, object>? valueConverter = null)
            where TData : class, new()
        {
            await excelMapper.SaveAsync(fileStream, objects, sheetName, true, valueConverter);
        }

        /// <summary>
        ///Excel 多sheet导出 有多个sheet且每个sheet类型不一致时
        /// </summary>
        /// <param name="excelMapper">导出配置</param>
        /// <param name="sheet">sheet</param>
        /// <returns></returns>
        public async Task ExportXlsxWithMultipleSheetsAddAsync<TSheetData>(ExcelMapper excelMapper,
            ZcyMultipleSheetInput<TSheetData> sheet)
        {
            using var ms = new MemoryStream(); // dummy
            await excelMapper.SaveAsync(ms, sheet.SheetData, sheet.SheetName);
        }

        /// <summary>
        /// Excel 多sheet导出  有多个sheet且每个sheet类型不一致时最后调用
        /// </summary>
        /// <param name="excelMapper">导出配置</param>
        /// <param name="absoluteFilePath">保存文件绝对路径</param>
        /// <param name="sheet">sheet</param>
        /// <returns></returns>
        public async Task ExportXlsxWithMultipleSheetsWriteAsync<TSheetData>(ExcelMapper excelMapper,
            string absoluteFilePath, ZcyMultipleSheetInput<TSheetData> sheet)
        {
            await excelMapper.SaveAsync(absoluteFilePath, sheet.SheetData, sheet.SheetName);
        }

        /// <summary>
        /// Excel 多sheet导出  有多个sheet且每个sheet类型不一致时最后调用(文件流)
        /// </summary>
        /// <param name="excelMapper">导出配置</param>
        /// <param name="fileStream">保存文件流</param>
        /// <param name="sheet">sheet</param>
        /// <returns></returns>
        public async Task ExportXlsxWithMultipleSheetsWriteAsync<TSheetData>(ExcelMapper excelMapper,
            Stream fileStream, ZcyMultipleSheetInput<TSheetData> sheet)
        {
            await excelMapper.SaveAsync(fileStream, sheet.SheetData, sheet.SheetName);
        }

        /// <summary>
        /// Excel导出自定义表头和多Sheet
        /// </summary>
        /// <returns></returns>
        public async Task<MemoryStream?> ExportXlsxWithMultipleHeadersAndSheetsWriteAsync(List<DynamicHeadersAndSheetsModel> rowData)
        {
            var memoryStream = new MemoryStream();
            IWorkbook workBook = new XSSFWorkbook(); // 默认为 .xlsx 格式
            // 遍历每个数据集，创建对应的工作表
            foreach (var sheetData in rowData)
            {
                var sheetName = sheetData.SheetsName;
                var rows = sheetData.SheetsData;

                // 创建新的工作表
                var sheet = workBook.CreateSheet(sheetName);

                // 动态获取表头
                var headers = new HashSet<string>();
                foreach (var row in rows)
                {
                    foreach (var key in row.Keys)
                    {
                        headers.Add(key);
                    }
                }

                // 写入表头
                var headerRow = sheet.CreateRow(0);
                var headerIndex = 0;
                foreach (var header in headers)
                {
                    headerRow.CreateCell(headerIndex++).SetCellValue(header);
                }

                // 写入数据
                var rowIndex = 1; // 从第二行开始写数据
                foreach (var row in rows)
                {
                    var dataRow = sheet.CreateRow(rowIndex++);
                    var colIndex = 0;
                    foreach (var header in headers)
                    {
                        if (row.TryGetValue(header, out var value))
                        {
                            dataRow.CreateCell(colIndex++).SetCellValue(value?.ToString());
                        }
                        else
                        {
                            dataRow.CreateCell(colIndex++).SetCellValue(string.Empty); // 如果没有该列值，写入空
                        }
                    }
                }
            }

            // 将工作簿写入到内存流
            workBook.Write(memoryStream, true);
            memoryStream.Position = 0; // 重置流的位置
            await Task.CompletedTask;
            return memoryStream;
        }

        /// <summary>
        /// 获取Excel单元格值(原始类型)
        /// </summary>
        /// <param name="dynamicDataList">Excel弱类型数据</param>
        /// <param name="rowNumber">Excel行号</param>
        /// <param name="column">Excel列名</param>
        /// <returns></returns>
        private object? GetCellValue(List<dynamic> dynamicDataList,
            int rowNumber, string column)
        {
            if (rowNumber > dynamicDataList.Count)
            {
                return default;
            }

            var currentRowData = dynamicDataList[rowNumber - 1] as IDictionary<string, object>;
            if (currentRowData == null ||
                currentRowData.ContainsKey(column) == false)
            {
                return default;
            }

            return currentRowData.First(a => a.Key == column).Value;
        }

        /// <summary>
        /// 单元格值(原始类型)转目标类型
        /// </summary>
        /// <remarks>
        ///  适用于Excel读取出来类型和目标类型不一致时，多用于目标类型为Int等
        /// </remarks>
        /// <param name="cellValue">单元格原始类型</param>
        /// <param name="targetPropertyInfo">目标字段信息</param>
        /// <returns></returns>
        private object CellValueChangeType(object cellValue, PropertyInfo targetPropertyInfo)
        {
            Type realityType = targetPropertyInfo.PropertyType;
            if (targetPropertyInfo.PropertyType.IsValueType &&
                targetPropertyInfo.PropertyType.IsGenericType &&
                targetPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                //可空类型转换时 需要真实数据类型
                realityType = targetPropertyInfo.PropertyType.GetGenericArguments().First();
            }

            try
            {
                return Convert.ChangeType(cellValue, realityType);
            }
            catch (FormatException foxEx)
            {
                throw new ArgumentException($"单元格值：{cellValue},格式化异常。目标类型为：{realityType.FullName}",
                    innerException: foxEx.InnerException);
            }
        }

        /// <summary>
        /// 获取具有单选或者复选的行数据
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string>? GetCheckAndRadioRow(IDictionary<string, object>? currentRow,
            IDictionary<string, object>? nextRow)
        {
            if (currentRow == null || nextRow == null)
            {
                return null;
            }

            var resultDic = new Dictionary<string, string>();

            //遍历当前行数据
            foreach (var currentDicItem in currentRow)
            {
                //默认__indexes__ 和 第一列 A 不需要
                if (currentDicItem.Key == "__indexes__" ||
                    currentDicItem.Key == "A")
                {
                    continue;
                }

                var resultKey = currentDicItem.Value + "";
                if (string.IsNullOrEmpty(resultKey))
                {
                    //当前行为空跳过
                    continue;
                }

                resultDic.Add(resultKey, nextRow[currentDicItem.Key] + "");
            }

            return resultDic;
        }

        /// <summary>
        /// 多选枚举处理
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="currentPropertyInfo">当前字段属性信息</param>
        /// <param name="result">返回结果</param>
        /// <param name="currentItem">当前行数据</param>
        /// <param name="nextRowData">下一行数据</param>
        private void FlagsEnumHandler<TData>(ZcyPropertyInfo currentPropertyInfo,
            TData result, dynamic currentItem, dynamic nextRowData)
        {
            //多选处理   当前行是枚举描述 第二行是“真值”对应的字段
            //当前行数据字典
            var currentDic = currentItem as IDictionary<string, object>;
            //下一行数据
            var nextRowDic = nextRowData as IDictionary<string, object>;
            var resultDic = GetCheckAndRadioRow(currentDic, nextRowDic);
            if (resultDic == null)
            {
                return;
            }

            //字段转为枚举
            var tempEnumV = currentPropertyInfo.PropertyInfoExt.PropertyType
                .ToFlagsEnumByEnumDesc(resultDic, currentPropertyInfo.WeGymColumn.TrueStr);
            currentPropertyInfo.PropertyInfoExt.SetValue(result, tempEnumV);
        }

        #endregion

        /// <summary>
        ///  动态Excel转实体类 适用于表头是A列
        /// </summary>
        /// <typeparam name="TData">输出类型</typeparam>
        /// <returns></returns>
        private TData ParseSpecialXlsx<TData>(IEnumerable<dynamic> dynamicDataList) where TData : class, new()
        {
            var result = new TData();

            var resultType = typeof(TData);
            var resultColumnMap = new List<ZcyPropertyInfo>();

            //1、读取所有共有属性字段
            foreach (var propertyInfo in resultType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var weGymColumn = propertyInfo.GetCustomAttribute<ZcyColumnAttribute>();
                if (weGymColumn == null ||
                    propertyInfo.CanWrite == false)
                {
                    continue;
                }

                resultColumnMap.Add(new ZcyPropertyInfo(weGymColumn, propertyInfo));
            }

            var skipRowNumber = new List<int>();

            //2、遍历表格数据
            var xlsxToList = dynamicDataList.ToList();
            for (int i = 0; i < xlsxToList.Count; i++)
            {
                if (skipRowNumber.Contains(i))
                {
                    continue;
                }

                var item = xlsxToList[i];
                if (item == null || string.IsNullOrEmpty(item.A))
                {
                    continue;
                }

                var title = item.A + "";
                //标题列对应的映射关系
                var currentColumnMap = resultColumnMap.FirstOrDefault(a => a.WeGymColumn.ColumnName == title);
                if (currentColumnMap == null)
                {
                    continue;
                }

                //当前类字段信息
                var currentPropertyInfo = currentColumnMap.PropertyInfoExt;
                if (currentPropertyInfo.IsEnumExt())
                {
                    if (currentPropertyInfo.IsFlagsEnumExt())
                    {
                        skipRowNumber.Add(i + 1);
                        //下一行数据
                        var nextRowItem = xlsxToList[i + 1];
                        FlagsEnumHandler(currentColumnMap, result, item, nextRowItem);
                        continue;
                    }

                    #region 单枚举处理
                    string enumDesc = item.B;
                    if (string.IsNullOrEmpty(enumDesc))
                    {
                        continue;
                    }

                    object? enumValue;
                    if (currentPropertyInfo.PropertyType.IsEnum)
                    {
                        //非可空枚举
                        enumValue = currentPropertyInfo.PropertyType.ToEnumByEnumDesc(enumDesc);
                    }
                    else
                    {
                        //可空枚举
                        enumValue = currentPropertyInfo.PropertyType
                            .GetGenericArguments()
                            .First().ToEnumByEnumDesc(enumDesc);
                    }
                    currentPropertyInfo.SetValue(result, enumValue);
                    #endregion
                }
                else
                {
                    var valueConvert = CellValueChangeType(item.B, currentColumnMap.PropertyInfoExt);
                    currentPropertyInfo.SetValue(result, valueConvert);
                }
            }

            //3、特殊字段处理
            foreach (var itemMap in resultColumnMap
                         .Where(a => a.WeGymColumn.SpecialRowNumber > 0 &&
                                     string.IsNullOrEmpty(a.WeGymColumn.SpecialColumn) == false))
            {
                var cellValue = GetCellValue(xlsxToList, itemMap.WeGymColumn.SpecialRowNumber,
                    itemMap.WeGymColumn.SpecialColumn);
                if (cellValue == null)
                {
                    continue;
                }

                var valueConvert = CellValueChangeType(cellValue, itemMap.PropertyInfoExt);
                itemMap.PropertyInfoExt.SetValue(result, valueConvert);
            }

            return result;
        }
    }
}
