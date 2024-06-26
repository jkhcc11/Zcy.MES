using System;

namespace Zcy.ExcelNpoi
{
	/// <summary>
	/// Excel列特性
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class ZcyColumnAttribute : Attribute
	{
		/// <summary>
		/// 普通模式
		/// </summary>
		/// <param name="columnName">列名</param>
		public ZcyColumnAttribute(string columnName)
		{
			ColumnName = columnName;
		}

		/// <summary>
		/// 枚举值多选模式
		/// </summary>
		/// <param name="columnName">列名</param>
		/// <param name="trueStr">真值对应的字符串</param>
		public ZcyColumnAttribute(string columnName, string trueStr)
		{
			ColumnName = columnName;
			TrueStr = trueStr;
		}

		/// <summary>
		/// 列名
		/// </summary>
		/// <remarks>
		///  一般为A列 上对应的名称
		/// </remarks>
		public string ColumnName { get; protected set; }

		/// <summary>
		/// 真值对应的字符串
		/// </summary>
		/// <remarks>
		///  Bool 类型真值 对应的字符串
		/// </remarks>
		public string? TrueStr { get; protected set; }

		/// <summary>
		/// 特殊字段对应的行号 配置<see cref="SpecialColumn"/> 一起使用 一个Excel表格仅读取一次
		/// </summary>
		/// <remarks>
		///  Excel对应的行号
		/// </remarks>
		public int SpecialRowNumber { get; set; }

		/// <summary>
		/// 特殊字段对应的列名
		/// </summary>
		/// <remarks>
		///  Excel对应的列名：A、B、C、D、E、F、G
		/// </remarks>
		public string? SpecialColumn { get; set; }
	}
}
