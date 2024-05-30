using System.Text.Json;

namespace Zcy.BaseInterface
{
    public static class JsonExtension
    {
        /// <summary>
        /// 转JsonStr
        /// </summary>
        /// <returns></returns>
        public static string ToJsonStrExt(this object? obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = false, // 紧密型，不使用换行和缩进
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // 不转义汉字
            };
            return JsonSerializer.Serialize(obj, options);
        }
    }
}
