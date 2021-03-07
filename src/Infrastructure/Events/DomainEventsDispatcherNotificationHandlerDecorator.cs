using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Events
{
    // When a a notification handler (outbox message or internal command) is run
    // we want to make sure to dispatch domain events after that command has been run just
    // like we do for regular ICommands or the mediatr IRequest
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
