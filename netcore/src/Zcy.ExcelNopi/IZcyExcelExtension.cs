using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ganss.Excel;

namespace Zcy.ExcelNpoi
{
    /// <summary>
    /// ExcelMapper扩展接口
    /// </summary>
    public interface IZcyExcelExtension
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
        Task<IEnumerable<TData>> ParseXlsxAsync<TData>(string filePath, bool isHasHeaderRow = true,
            bool skipBlankRows = true,
            int minRowNumber = 0, int maxRowNumber = int.MaxValue, int sheetIndex = 0) where TData : class, new();

        Task<IEnumerable<TData>> ParseXlsxAsync<TData>(Stream fileStream, bool isHasHeaderRow = true,
            bool skipBlankRows = true,
            int minRowNumber = 0, int maxRowNumber = int.MaxValue, int sheetIndex = 0) where TData : class, new();

        Task<IEnumerable<TData>> ParseXlsxAsync<TData>(Stream fileStream, string sheetName,
            bool isHasHeaderRow = true, bool skipBlankRows = true,
            int minRowNumber = 0, int maxRowNumber = int.MaxValue) where TData : class, new();

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
        Task<IEnumerable<dynamic>> ParseXlsxWithDynamicAsync(string filePath, bool isHasHeaderRow = true,
            bool skipBlankRows = true,
            int minRowNumber = 0, int maxRowNumber = int.MaxValue, int sheetIndex = 0);

        Task<IEnumerable<dynamic>> ParseXlsxWithDynamicAsync(Stream fileStream, bool isHasHeaderRow = true,
            bool skipBlankRows = true,
            int minRowNumber = 0, int maxRowNumber = int.MaxValue, int sheetIndex = 0);

        Task<IEnumerable<dynamic>> ParseXlsxWithDynamicAsync(Stream fileStream, string sheetName,
            bool isHasHeaderRow = true, bool skipBlankRows = true,
            int minRowNumber = 0, int maxRowNumber = int.MaxValue);

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
        Task<TData> ParseSpecialXlsxAsync<TData>(string filePath, bool isHasHeaderRow = true,
            bool skipBlankRows = true, int minRowNumber = 0,
            int maxRowNumber = int.MaxValue, int sheetIndex = 0) where TData : class, new();

        Task<TData> ParseSpecialXlsxAsync<TData>(Stream fileStream, bool isHasHeaderRow = true,
            bool skipBlankRows = true, int minRowNumber = 0,
            int maxRowNumber = int.MaxValue, int sheetIndex = 0) where TData : class, new();

        /// <summary>
        /// Excel 普通导出
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="absoluteFilePath">保存文件绝对路径</param>
        /// <param name="objects">导出得数据</param>
        /// <param name="sheetName">sheet名 默认：sheet1</param>
        /// <param name="valueConverter">值转换</param>
        /// <returns></returns>
        Task ExportXlsxAsync<TData>(string absoluteFilePath, IEnumerable<TData> objects,
            string sheetName = "sheet1", Func<string, object, object>? valueConverter = null)
            where TData : class, new();

        /// <summary>
        /// Excel 普通导出(文件流)
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="fileStream">保存文件流</param>
        /// <param name="objects">导出得数据</param>
        /// <param name="sheetName">sheet名 默认：sheet1</param>
        /// <param name="valueConverter">值转换</param>
        /// <returns></returns>
        Task ExportXlsxAsync<TData>(Stream fileStream, IEnumerable<TData> objects, string sheetName = "sheet1",
            Func<string, object, object>? valueConverter = null)
            where TData : class, new();

        /// <summary>
        /// Excel 模板导出
        /// </summary>
        /// <typeparam name="TData">数据类型</typeparam>
        /// <param name="tplAbsoluteFilePath">模板文件绝对路径</param>
        /// <param name="absoluteFilePath">保存文件绝对路径</param>
        /// <param name="objects">导出得数据</param>
        /// <param name="sheetName">sheet必须跟模板sheet一致</param>
        /// <param name="valueConverter">值转换</param>
        /// <returns></returns>
        Task ExportXlsxWithTplAsync<TData>(string tplAbsoluteFilePath, string absoluteFilePath,
            IEnumerable<TData> objects, string sheetName,
            Func<string, object, object>? valueConverter = null)
            where TData : class, new();

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
        Task ExportXlsxWithTplAsync<TData>(string tplAbsoluteFilePath, Stream fileStream,
            IEnumerable<TData> objects, string sheetName,
            Func<string, object, object>? valueConverter = null)
            where TData : class, new();

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
        Task ExportXlsxWithCustomAsync<TData>(ExcelMapper excelMapper, string absoluteFilePath,
            IEnumerable<TData> objects, string sheetName,
            Func<string, object, object>? valueConverter = null)
            where TData : class, new();

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
        Task ExportXlsxWithCustomAsync<TData>(ExcelMapper excelMapper, Stream fileStream,
            IEnumerable<TData> objects, string sheetName,
            Func<string, object, object>? valueConverter = null)
            where TData : class, new();

        /// <summary>
        /// Excel 多sheet导出 有多个sheet且每个sheet类型不一致时
        /// </summary>
        /// <param name="excelMapper">导出配置</param>
        /// <param name="sheet">sheet</param>
        /// <returns></returns>
        Task ExportXlsxWithMultipleSheetsAddAsync<TSheetData>(ExcelMapper excelMapper,
            ZcyMultipleSheetInput<TSheetData> sheet);

        /// <summary>
        /// Excel 多sheet导出  有多个sheet且每个sheet类型不一致时最后调用
        /// </summary>
        /// <param name="excelMapper">导出配置</param>
        /// <param name="absoluteFilePath">保存文件绝对路径</param>
        /// <param name="sheet">sheet</param>
        /// <returns></returns>
        Task ExportXlsxWithMultipleSheetsWriteAsync<TSheetData>(ExcelMapper excelMapper,
            string absoluteFilePath, ZcyMultipleSheetInput<TSheetData> sheet);

        /// <summary>
        /// Excel 多sheet导出  有多个sheet且每个sheet类型不一致时最后调用(文件流)
        /// </summary>
        /// <param name="excelMapper">导出配置</param>
        /// <param name="fileStream">保存文件流</param>
        /// <param name="sheet">sheet</param>
        /// <returns></returns>
        Task ExportXlsxWithMultipleSheetsWriteAsync<TSheetData>(ExcelMapper excelMapper,
            Stream fileStream, ZcyMultipleSheetInput<TSheetData> sheet);

        /// <summary>
        /// Excel导出自定义表头和多Sheet
        /// </summary>
        /// <returns></returns>
        Task<MemoryStream?> ExportXlsxWithMultipleHeadersAndSheetsWriteAsync(List<DynamicHeadersAndSheetsModel> rowData);

    }
}
