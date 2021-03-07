using SampleStore.Domain.SharedKernel.Abstractions.TypedIds;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleStore.Application.Configuration.StronglyTypedIds
{

    // deserialize reads the value and converts it the our strongly-typed Id. 
    // serialize just writes out the strongly-typed id's Value property.
    public class StronglyTypedIdJsonConverter<TStronglyTypedId, TValue> : JsonConverter<TStronglyTypedId>
        where TStronglyTypedId : StronglyTypedId<TValue>
        where TValue : notnull
    {
        public override TStronglyTypedId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
            {
                return null;
            }

            var value = JsonSerializer.Deserialize<TValue>(ref reader, options);
            var factory = StronglyTypedIdHelper.GetFactory<TValue>(typeToConvert);
            return (TStronglyTypedId)factory(value);
        }

        public override void Write(Utf8JsonWriter writer, TStronglyTypedId value, JsonSerializerOptions options)
        {
            if (value is not null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, value.Value, options);
            }
        }
    }
}
