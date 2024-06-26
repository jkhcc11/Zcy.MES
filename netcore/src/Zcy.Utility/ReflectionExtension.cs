using System;
using System.Linq;
using System.Reflection;

namespace Zcy.Utility
{
	/// <summary>
	/// 反射扩展
	/// </summary>
	public static class ReflectionExtension
	{
		/// <summary>
		/// 是否为枚举类型
		/// </summary>
		/// <param name="currentPropertyInfo">属性信息</param>
		/// <returns>
		///  为 True 时包含可空枚举 
		/// </returns>
		public static bool IsEnumExt(this PropertyInfo currentPropertyInfo)
		{
			if (currentPropertyInfo.PropertyType.IsEnum)
			{
				//非可空枚举
				return true;
			}

			//可空枚举
			return currentPropertyInfo.PropertyType.IsValueType &&
				   currentPropertyInfo.PropertyType.IsGenericType &&
				   currentPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
				   currentPropertyInfo.PropertyType.GetGenericArguments().First().IsEnum;
		}

		/// <summary>
		/// 是否为Flags枚举类型
		/// </summary>
		/// <param name="currentPropertyInfo">属性信息</param>
		/// <returns>
		///  为 True 时包含可空枚举 
		/// </returns>
		public static bool IsFlagsEnumExt(this PropertyInfo currentPropertyInfo)
		{
			var flagsAttributes = currentPropertyInfo.PropertyType.GetCustomAttribute<FlagsAttribute>();
			if (flagsAttributes == null)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 是否为可空值类型
		/// </summary>
		/// <param name="currentType">当前类型</param>
		public static bool IsValueTypeCanNullExt(this Type currentType)
		{
			return currentType.IsValueType &&
				   currentType.IsGenericType &&
				   currentType.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		/// <summary>
		/// 获取可空值类型的实际类型
		/// </summary>
		/// <param name="currentType">当前类型</param>
		public static Type GetValueTypeCanNullExt(this Type currentType)
		{
			if (currentType.IsGenericType == false)
			{
				return currentType;
			}

			return currentType.GetGenericArguments().First();
		}
	}
}
