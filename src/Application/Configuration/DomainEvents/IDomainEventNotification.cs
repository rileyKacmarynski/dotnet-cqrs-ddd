using MediatR;
using System;

namespace SampleStore.Application.Configuration.DomainEvents
{
    public interface IDomainEventNotification<out TEventType> : IDomainEventNotifcation
    {
        TEventType DomainEvent { get; }
    }

    public interface IDomainEventNotifcation : INotification
    {
        Guid Id { get; }
    }
}