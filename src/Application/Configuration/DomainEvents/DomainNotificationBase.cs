using SampleStore.Domain.SharedKernel.Abstractions;
using System;
using System.Text.Json.Serialization;

namespace SampleStore.Application.Configuration.DomainEvents
{
    // A domain notification is created from a domain event. Raised domain events are handled
    // within the same transaction has the original command. DomainNotifications are saved to the outbox
    // and processed at a later time.
    public class DomainNotificationBase<T> : IDomainEventNotification<T> where T : IDomainEvent
    {
        [JsonIgnore]
        public T DomainEvent { get; }

        public Guid Id { get; }

        public DomainNotificationBase(T domainEvent)
        {
            Id = Guid.NewGuid();
            DomainEvent = domainEvent;
        }
    }
}