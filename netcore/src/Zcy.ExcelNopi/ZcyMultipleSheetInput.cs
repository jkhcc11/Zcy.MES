using System.Collections.Generic;

namespace Zcy.ExcelNpoi
{
	/// <summary>
	/// 多Sheet导出 Input
	/// </summary>
	public class ZcyMultipleSheetInput<TSheetData>
	{
        public ZcyMultipleSheetInput(string sheetName, List<TSheetData> sheetData)
        {
            SheetName = sheetName;
            SheetData = sheetData;
        }

        /// <summary>
		/// Sheet名
		/// </summary>
		public string SheetName { get; set; }

		/// <summary>
		/// Sheet数据
		/// </summary>
		public List<TSheetData> SheetData { get; set; }
	}
}
