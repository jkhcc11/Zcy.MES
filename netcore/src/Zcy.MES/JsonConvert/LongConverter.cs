using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zcy.MES.JsonConvert
{
    public class LongConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                //兼容入参long型不加引号
                return reader.GetInt64();
            }

            long.TryParse(reader.GetString(), out long temp);
            return temp;
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }

}
