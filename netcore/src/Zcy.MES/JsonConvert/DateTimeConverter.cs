using System.Text.Json;
using System.Text.Json.Serialization;
using Zcy.BaseInterface;

namespace Zcy.MES.JsonConvert
{
    /// <summary>
    /// Json时间格式化 
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        //兼容前端传两种格式时间
        private static readonly string[] DateFormats = {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd"
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString() ?? string.Empty;
            foreach (var format in DateFormats)
            {
                if (DateTime.TryParseExact(dateString, format,
                        null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
            }

            throw new JsonException($"DateTimeConverter Unable to parse \"{dateString}\" as a date.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToLocalTime().ToString(ZcyMesConst.DateTimeFormat));
    }

}
