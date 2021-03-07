using Microsoft.Extensions.DependencyInjection;
using SampleStore.Application.Customers.IntegrationHandlers;
using SampleStore.Domain.SharedKernel.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace SampleStore.Application.Configuration.DomainEvents
{
    public class DomainEventNotificationFactory : IDomainEventNotificationFactory
    {
        // using ConcurrentBag because this class is registered as a singleton. I think this is the right way
        // to do this sort of thing. 
        private static readonly ConcurrentBag<Type> _notificationTypes = new ConcurrentBag<Type>();
        private readonly IServiceProvider _serviceProvider;

        public DomainEventNotificationFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var types = typeof(CustomerRegisteredNotification)
                .Assembly
                .GetTypes()
                .Where(t => t.IsDomainEventNotification());

            foreach (var type in types)
            {
                _notificationTypes.Add(type);
            }
        }

        public IDomainEventNotification<IDomainEvent> GetNotification(IDomainEvent domainEvent)
        {
            // this gives us the IDomainEventNofication open generic type
            var domainEventNotificationType = typeof(IDomainEventNotification<>);

            // we can add the type to that IDomainEventNotification open generic making it a closed
            // generic (I think). In this case IDomainEventNotification<CustomerRegisteredDomainEvent>
            var domainNotificationWithGenericType = domainEventNotificationType.MakeGenericType(domainEvent.GetType());

            // use the type above to find the class that implements IDomainEventNotification<CustomerRegisteredEvent>
            // which is CustomerRegisteredNotification
            var domainNotificationType = _notificationTypes
                .Where(t => t.GetInterfaces()
                    .Contains(domainNotificationWithGenericType))
                .SingleOrDefault();

            return domainNotificationType is not null
                ? ActivatorUtilities.CreateInstance(_serviceProvider, domainNotificationType, domainEvent) as IDomainEventNotification<IDomainEvent>
                : null;
        }
    }

    public static class TypeExtensions
    {
        public static bool IsDomainEventNotification(this Type t)
        {
            return t.GetInterfaces()
                .Contains(typeof(IDomainEventNotification)) 
                && !t.IsInterface 
                && !t.IsGenericType;      // don't include the generic DomainNotificationBase class.
        }
    }
}