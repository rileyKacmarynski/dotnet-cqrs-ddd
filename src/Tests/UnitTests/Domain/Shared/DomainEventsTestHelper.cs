using SampleStore.Domain.SharedKernel.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Domain.Shared
{
    public class DomainEventsTestHelper
    {
        public static List<IDomainEvent> GetAllDomainEvents(Entity aggregate)
        {
            var domainEvents = new List<IDomainEvent>();

            if(aggregate.DomainEvents != null)
            {
                domainEvents.AddRange(aggregate.DomainEvents);
            }

            var fields = GetFields(aggregate);

            // loop through each field. If it's an entity, or a collection of 
            // entities call GetAllDomainEvents with those entites.
            foreach(var field in fields)
            {
                if (field.FieldType.IsAssignableFrom(typeof(Entity)))
                {
                    var entity = field.GetValue(aggregate) as Entity;
                    domainEvents.AddRange(GetAllDomainEvents(entity));
                }

                if(field.FieldType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(field.FieldType))
                {
                    if(field.GetValue(aggregate) is IEnumerable enumerable)
                    {
                        foreach(var item in enumerable)
                        {
                            if(item is Entity entityItem)
                            {
                                domainEvents.AddRange(GetAllDomainEvents(entityItem));
                            }
                        }
                    }
                }
            }

            return domainEvents;
        }

        public static void ClearAllDomainEvents(Entity aggregate)
        {
            aggregate.ClearDomainEvents();

            var fields = GetFields(aggregate);

            foreach(var field in fields)
            {
                if (field.FieldType.IsAssignableFrom(typeof(Entity)))
                {
                    var entity = field.GetValue(aggregate) as Entity;
                    ClearAllDomainEvents(entity);
                }

                if (field.FieldType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(field.FieldType))
                {
                    if (field.GetValue(aggregate) is IEnumerable enumerable)
                    {
                        foreach (var item in enumerable)
                        {
                            if (item is Entity entityItem)
                            {
                                ClearAllDomainEvents(entityItem);
                            }
                        }
                    }
                }
            }
        }

        private static FieldInfo[] GetFields(Entity aggregate)
        {
            return aggregate.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .Concat(aggregate.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)).ToArray();
        }
    }
}
