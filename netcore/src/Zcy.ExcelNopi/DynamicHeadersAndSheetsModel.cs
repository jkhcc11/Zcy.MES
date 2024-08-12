using System.Collections.Generic;

namespace Zcy.ExcelNpoi
{
    /// <summary>
    /// Excel动态头部和多Sheet
    /// </summary>
    public class DynamicHeadersAndSheetsModel
    {
        /// <summary>
        /// Excel动态头部和多Sheet
        /// </summary>
        /// <param name="sheetsName">工作表名称</param>
        /// <param name="sheetsData">工作表数据</param>
        public DynamicHeadersAndSheetsModel(string sheetsName, 
            List<Dictionary<string, object>> sheetsData)
        {
            SheetsName = sheetsName;
            SheetsData = sheetsData;
        }

        /// <summary>
        /// 工作表名称
        /// </summary>
        public string SheetsName { get; set; }

        /// <summary>
        /// 工作表数据
        /// </summary>
        public List<Dictionary<string, object>> SheetsData { get; set; }
    }
}
