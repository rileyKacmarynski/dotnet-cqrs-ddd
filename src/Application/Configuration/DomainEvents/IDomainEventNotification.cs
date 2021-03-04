using MediatR;
using System;

namespace SampleStore.Application.Configuration.DomainEvents
{
    // the out keyword means that TEventType is covariant. 
    // this means we can use a more derived type for TEventType than what
    // is specified by the generic parameter. 
    // I think this is what let's us toss CustomerRegisteredNotification into an IEnumerable<IDomainNotification<IDomainEvent>>
    public interface IDomainEventNotification<out TEventType> : IDomainEventNotification
    {
        TEventType DomainEvent { get; }
    }

    public interface IDomainEventNotification : INotification
    {
        Guid Id { get; }
    }
}