using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Events
{
    // I think this is for when there's a domain event or internal event handler that kicks off 
    // additional domain events. Our other decorator that dispatches domain events only decorates
    // command handlers, not notification handlers. 
    public class DomainEventsDispatcherNotificationHandlerDecorator<T> : INotificationHandler<T> where T : INotification
    {
        private readonly IDomainEventsDispatcher _domainEventDispatcher;
        private readonly INotificationHandler<T> _decorated;

        public DomainEventsDispatcherNotificationHandlerDecorator(IDomainEventsDispatcher domainEventDispatcher, INotificationHandler<T> decorated)
        {
            _domainEventDispatcher = domainEventDispatcher;
            _decorated = decorated;
        }

        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            await _decorated.Handle(notification, cancellationToken);
            await _domainEventDispatcher.DispatchEventAsync();
        }
    }
}
