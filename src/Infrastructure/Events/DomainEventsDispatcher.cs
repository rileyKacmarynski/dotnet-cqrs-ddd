using MediatR;
using Newtonsoft.Json;
using SampleStore.Application.Configuration.DomainEvents;
using SampleStore.Domain.SharedKernel.Abstractions;
using SampleStore.Infrastructure.Database;
using SampleStore.Infrastructure.Events.Outbox;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Events
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly IDomainEventNotificationFactory _domainEventNotificationFactory;
        private readonly OrdersContext _ordersContext;

        public DomainEventsDispatcher(IMediator mediator, IDomainEventNotificationFactory domainEventNotificationFactory, OrdersContext ordersContext)
        {
            _mediator = mediator;
            _domainEventNotificationFactory = domainEventNotificationFactory;
            _ordersContext = ordersContext;
        }

        public async Task DispatchEventAsync()
        {
            var domainEntities = _ordersContext.ChangeTracker
                .Entries<Entity>()
                .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            var domainEventNotifications = new List<IDomainEventNotification<IDomainEvent>>();

            // take each domain event and create the corresponding domain event notification
            // the notification is the piece that will be called asynchronously
            // lets say we have a CustomerRegisteredDomainEvent
            foreach (var domainEvent in domainEvents)
            {
                var domainNotification = _domainEventNotificationFactory.GetNotification(domainEvent);
                if (domainNotification != null)
                {
                    domainEventNotifications.Add(domainNotification);
                }
            }

            domainEntities.ForEach(entry => entry.Entity.ClearDomainEvents());

            // process each domain event (in the current transaction)
            var tasks = domainEvents
                .Select(async (domainEvent) => await _mediator.Publish(domainEvent));

            await Task.WhenAll(tasks);

            // once that's done write the domain event notifications to the outbox
            foreach (var domainEventNotification in domainEventNotifications)
            {
                string type = domainEventNotification.GetType().FullName;
                var data = JsonConvert.SerializeObject(domainEventNotification);

                var outboxMessage = new OutboxMessage(
                    domainEventNotification.DomainEvent.OccuredOn,
                    type,
                    data);

                _ordersContext.OutboxMessages.Add(outboxMessage);
            }
        }
    }
}
