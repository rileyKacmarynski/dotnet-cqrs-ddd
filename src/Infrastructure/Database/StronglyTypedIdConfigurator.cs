using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SampleStore.Domain.SharedKernel.Abstractions.TypedIds;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace SampleStore.Infrastructure.Database
{
    // to get stronly-typed Ids to we have to use the .HasCoversion method to map from our value to the strongly-typed Id. 
    // builder.Property(p => p.Id).HasCoversion(id => id.Value, value => new CustomerId(value));
    // the above code is a pain to write for each entity, so this class uses reflection to do it for us. 
    // https://thomaslevesque.com/2020/12/23/csharp-9-records-as-strongly-typed-ids-part-4-entity-framework-core-integration/
    internal static class StronglyTypedIdConfigurator
    {
        public static void AddStronglyTypedIdConversions(ModelBuilder modelBuilder)
        {
            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach(var property in entityType.GetProperties())
                {
                    if(StronglyTypedIdHelper.IsStronglyTypedId(property.ClrType, out var valueType))
                    {
                        var converter = StronglyTypedIdConverters.GetOrAdd(
                            property.ClrType,
                            _ => CreateStronglyTypedIdConverter(property.ClrType, valueType));

                        property.SetValueConverter(converter);
                    }
                }
            }
        }

        private static readonly ConcurrentDictionary<Type, ValueConverter> StronglyTypedIdConverters = new();

        private static ValueConverter CreateStronglyTypedIdConverter(Type stronglyTypedIdType, Type valueType)
        {
            // value => id.Value
            var toProviderExpression = BuildToProviderExpression(stronglyTypedIdType, valueType);

            // value => new CustomerId(value)
            var fromProviderExpression = BuildFromProviderExpression(stronglyTypedIdType, valueType);

            var converterType = typeof(ValueConverter<,>)
                .MakeGenericType(stronglyTypedIdType, valueType);

            return (ValueConverter)Activator.CreateInstance(converterType, toProviderExpression, fromProviderExpression, null);
        }

        private static LambdaExpression BuildFromProviderExpression(Type stronglyTypedIdType, Type valueType)
        {
            var fromProviderFuncType = typeof(Func<,>)
                .MakeGenericType(valueType, stronglyTypedIdType);
            var valueParam = Expression.Parameter(valueType, "value");
            var ctor = stronglyTypedIdType.GetConstructor(new[] { valueType });
            var fromProviderExpression = Expression.Lambda(
                fromProviderFuncType,
                Expression.New(ctor, valueParam),
                valueParam);
            return fromProviderExpression;
        }

        private static LambdaExpression BuildToProviderExpression(Type stronglyTypedIdType, Type valueType)
        {
            var toProviderFuncType = typeof(Func<,>)
                .MakeGenericType(stronglyTypedIdType, valueType);
            var stronglyTypedIdParam = Expression.Parameter(stronglyTypedIdType, "id");
            var toProviderExpression = Expression.Lambda(
                toProviderFuncType,
                Expression.Property(stronglyTypedIdParam, "Value"),
                stronglyTypedIdParam);
            return toProviderExpression;
        }
    } 
}
