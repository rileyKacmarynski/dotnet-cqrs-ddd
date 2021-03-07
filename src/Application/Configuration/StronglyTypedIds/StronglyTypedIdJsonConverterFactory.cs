using SampleStore.Domain.SharedKernel.Abstractions.TypedIds;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleStore.Application.Configuration.StronglyTypedIds
{
    // We have to add the converter as a closed type, which means we have to specify the StronglyTyped id type
    // ex.  new StronglyTypedIdJsonConverter<CustomerId, string>()
    // We can create a factory to pass back the right converter for each strongly-typed id type at runtime
    public class StronglyTypedIdJsonConverterFactory : JsonConverterFactory
    {
        private static ConcurrentDictionary<Type, JsonConverter> Cache = new();

        public override bool CanConvert(Type typeToConvert)
        {
            return StronglyTypedIdHelper.IsStronglyTypedId(typeToConvert);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            if (!StronglyTypedIdHelper.IsStronglyTypedId(typeToConvert, out var valueType))
            {
                throw new InvalidOperationException($"Cannot create converter for '{typeToConvert}'");
            }

            var type = typeof(StronglyTypedIdJsonConverter<,>).MakeGenericType(typeToConvert, valueType);
            return (JsonConverter)Activator.CreateInstance(type);
        }
    }
}
