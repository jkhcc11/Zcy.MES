using System.Text.Json;
using System.Text.Json.Serialization;
using Zcy.BaseInterface;

namespace Zcy.MES.JsonConvert
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateTime.ParseExact(reader.GetString() ?? string.Empty, ZcyMesConst.DateTimeFormat, null);

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToLocalTime().ToString(ZcyMesConst.DateTimeFormat));
    }

}
