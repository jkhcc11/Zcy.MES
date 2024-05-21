using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace Zcy.MongoDB
{
    public class DecimalSerializer : SerializerBase<decimal>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, decimal value)
        {
            context.Writer.WriteDecimal128(new Decimal128(value));
        }

        public override decimal Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var decimal128 = context.Reader.ReadDecimal128();
            return (decimal)decimal128;
        }
    }
}
