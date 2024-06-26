using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Zcy.Utility
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举DisplayName
        /// </summary>
        /// <param name="enumType">枚举</param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum enumType)
        {
            var str = enumType.ToString();
            var field = enumType.GetType().GetField(str);
            var customAttributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (customAttributes.Length == 0) return str;
            var da = (DisplayAttribute)customAttributes[0];
            return da.Name;
        }

        /// <summary>
        /// 字符串拼接枚举 转 Flags枚举值
        /// </summary>
        /// <remarks>
        ///  如果是枚举名称需要区分大小写，如果是枚举值拼接可直接使用
        /// </remarks>
        /// <typeparam name="TEnum">枚举</typeparam>
        /// <param name="enumStr">以，逗号拼接的多枚举值字符串</param>
        /// <returns></returns>
        public static TEnum ToFlagsEnum<TEnum>(this string enumStr)
            where TEnum : Enum
        {
            if (string.IsNullOrEmpty(enumStr))
            {
                return default!;
            }

            enumStr = enumStr.Trim();
            int sum = 0;
            foreach (var str in enumStr.Split(','))
            {
                sum += (int)Enum.Parse(typeof(TEnum), str);
            }

            return (TEnum)Enum.Parse(typeof(TEnum), sum + "");
        }

        /// <summary>
        /// 根据枚举<see cref="DescriptionAttribute.Description"/> 值转换为枚举
        /// </summary>
        /// <typeparam name="TEnum">枚举</typeparam>
        /// <param name="enumDesc">枚举描述</param>
        /// <param name="defaultValue">为空时 默认值</param>
        /// <returns></returns>
        public static TEnum ToEnumByEnumDesc<TEnum>(this string enumDesc, TEnum defaultValue = default!)
            where TEnum : Enum
        {
            if (string.IsNullOrEmpty(enumDesc))
            {
                return defaultValue;
            }

            enumDesc = enumDesc.Trim();
            foreach (TEnum itemEnum in typeof(TEnum).GetEnumValues())
            {
                var fieldInfo = itemEnum.GetType().GetField(itemEnum.ToString());
                if (fieldInfo == null)
                {
                    return defaultValue;
                }

                //描述属性
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Any() == false)
                {
                    continue;
                }

                //描述
                var desc = ((DescriptionAttribute)attrs.First()).Description;
                if (desc == enumDesc)
                {
                    return itemEnum;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 根据枚举<see cref="DescriptionAttribute.Description"/> 值转换为枚举
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="enumDesc">枚举描述</param>
        /// <returns></returns>
        public static object? ToEnumByEnumDesc(this Type enumType, string enumDesc)
        {
            foreach (var fieldInfo in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                //描述属性
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Any() == false)
                {
                    continue;
                }

                //描述
                var desc = ((DescriptionAttribute)attrs.First()).Description;
                if (desc == enumDesc)
                {
                    return fieldInfo.GetValue(null);
                }
            }

            return default;
        }

        /// <summary>
        /// 根据字典对应关系转换为多枚举类型
        /// </summary>
        /// <param name="enumType">多枚举类型</param>
        /// <param name="originData">数据源 字典</param>
        /// <param name="trueStr">真值对应的字符串值</param>
        /// <returns></returns>
        public static object? ToFlagsEnumByEnumDesc(this Type enumType,
            Dictionary<string, string> originData, 
            string? trueStr = "是")
        {
            if (enumType.IsValueTypeCanNullExt())
            {
                if (originData.Values.Any(a => a == trueStr) == false)
                {
                    return default;
                }

                //变更为实际类型
                enumType = enumType.GetValueTypeCanNullExt();
            }

            var result = new List<object>();
            foreach (var originItem in originData)
            {
                if (originItem.Value != trueStr)
                {
                    continue;
                }

                var enumValue = enumType.ToEnumByEnumDesc(originItem.Key);
                if (enumValue == null)
                {
                    continue;
                }

                result.Add(enumValue);
            }

            if (result.Any() == false)
            {
                return default;
            }

            var tempInt = result.Aggregate((a, b) => (int)a | (int)b);
            return Enum.Parse(enumType, tempInt + "");
        }

        /// <summary>
        /// 枚举描述拼接枚举 转 Flags枚举值
        /// </summary>
        /// <remarks>
        ///  如果是枚举名称需要区分大小写，如果是枚举值拼接可直接使用
        /// </remarks>
        /// <typeparam name="TEnum">枚举</typeparam>
        /// <param name="enumDescStr">以，逗号拼接的多枚举描述字符串</param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        public static TEnum ToFlagsEnumByDesc<TEnum>(this string enumDescStr, TEnum defaultEnum = default!)
            where TEnum : Enum
        {
            if (string.IsNullOrEmpty(enumDescStr))
            {
                return default!;
            }

            enumDescStr = enumDescStr.Trim();
            var partArray = enumDescStr
                .Split(new[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.ToEnumByEnumDesc(defaultEnum).GetHashCode())
                .Where(a => a.GetHashCode() > 0)
                .Distinct()
                .ToArray();
            return string.Join(",", partArray).ToFlagsEnum<TEnum>();
        }
    }
}
