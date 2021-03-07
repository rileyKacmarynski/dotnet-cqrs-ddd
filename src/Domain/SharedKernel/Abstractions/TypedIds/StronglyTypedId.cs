using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SampleStore.Domain.SharedKernel.Abstractions.TypedIds
{
    // https://thomaslevesque.com/2020/10/30/using-csharp-9-records-as-strongly-typed-ids/

    // the type converter will allow for mvc model binding to happen between a string and our strongly-typed Id
    // this will also tell Newtonsoft.Json to serialize the stongly-typed id to a string. 
    // Newtonsoft looks for a JsonConverter, and if it can't find one for the type it uses the TypeConverter
    [TypeConverter(typeof(StronglyTypedIdConverter))]
    public abstract record StronglyTypedId<TValue>(TValue Value) where TValue : notnull
    {
        public override string ToString() => Value.ToString();
    }
}
