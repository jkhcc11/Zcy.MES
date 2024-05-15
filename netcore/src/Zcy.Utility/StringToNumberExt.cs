namespace Zcy.Utility
{
    /// <summary>
    /// 字符串转数字相关
    /// </summary>
    public static class StringToNumberExt
    {
        /// <summary>
        /// 字符串转Int
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int TryToInt32(this string str)
        {
            int.TryParse(str, out var r);
            return r;
        }

        /// <summary>
        /// 字符串转Int64
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static long TryToInt64(this string str)
        {
            long.TryParse(str, out var r);
            return r;
        }

        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static double TryToDouble(this string str)
        {
            double.TryParse(str, out var r);
            return r;
        }
    }
}
