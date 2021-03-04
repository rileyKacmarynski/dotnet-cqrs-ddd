using SampleStore.Domain.SharedKernel.Abstractions;

namespace SampleStore.Application.Configuration.DomainEvents
{
    public interface IDomainEventNotificationFactory
    {
        IDomainEventNotification<IDomainEvent> GetDomainEventNotification(IDomainEvent domainEvent);
    }
}