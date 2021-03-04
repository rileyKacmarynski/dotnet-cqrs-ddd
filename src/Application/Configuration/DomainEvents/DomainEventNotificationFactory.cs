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

        private static readonly ConcurrentBag<Type> _notificationTypes = new ConcurrentBag<Type>();
        private readonly IServiceProvider _serviceProvider;

        public IDomainEventNotification<IDomainEvent> GetDomainEventNotification(IDomainEvent domainEvent)
        {
            // this gives us the IDomainEventNofication open generic type
            var domainEventNotificationType = typeof(IDomainEventNotification<>);

            // we can add the type to that IDomainEventNotification open generic making it a closed
            // generic (I think). In this case IDomainEventNotification<CustomerRegisteredDomainEvent>
            var domainNotificationWithGenericType = domainEventNotificationType.MakeGenericType(domainEvent.GetType());


            var domainNotificationType = _notificationTypes
                .Where(t => t.GetInterfaces()
                    .Contains(domainNotificationWithGenericType))
                .SingleOrDefault();

            // I'm not sure if this will instantiate the proper CustomerRegisteredNotification
            // we have to work with DI to instantiate the a DomainEventNotification<CustomerRegisteredDomainEvent>

            // Do I have to register all DomainEventNotifications with DI to be able to do this or just DomainNotificationBase
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