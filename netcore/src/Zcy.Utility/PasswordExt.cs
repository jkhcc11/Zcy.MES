using System.Security.Cryptography;
using System.Text;

namespace Zcy.Utility
{
    public static class PasswordExt
    {
        /// <summary>
        /// 转Md5
        /// </summary>
        /// <param name="str">源</param>
        /// <param name="slot">盐</param>
        /// <returns></returns>
        public static string ToMd5(this string str,
            string slot = "U2FsdGVkX1/0BrXJSaS3r/ivTTZa+E7MeV+4vC8R28E=")
        {
            var hash = SHA256.Create();
            var data = hash.ComputeHash(Encoding.UTF8.GetBytes(str + slot));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            // 返回十六进制字符串
            return sBuilder.ToString();
        }
    }
}
