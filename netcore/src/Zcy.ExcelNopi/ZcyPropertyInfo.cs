using System.Reflection;

namespace Zcy.ExcelNpoi
{
	/// <summary>
	/// 类属性信息
	/// </summary>
	internal class ZcyPropertyInfo
    {
		/// <summary>
		/// 构造
		/// </summary>
		/// <param name="weGymColumn">列特性扩展</param>
		/// <param name="propertyInfoExt">类属性字段信息</param>
		public ZcyPropertyInfo(ZcyColumnAttribute weGymColumn, PropertyInfo propertyInfoExt)
		{
			WeGymColumn = weGymColumn;
			PropertyInfoExt = propertyInfoExt;
		}

		/// <summary>
		/// 列特性扩展
		/// </summary>
		public ZcyColumnAttribute WeGymColumn { get; set; }

		/// <summary>
		/// 类属性字段信息
		/// </summary>
		public PropertyInfo PropertyInfoExt { get; set; }
	}
}
