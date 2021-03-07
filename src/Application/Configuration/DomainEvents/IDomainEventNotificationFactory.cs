using SampleStore.Domain.SharedKernel.Abstractions;

namespace SampleStore.Application.Configuration.DomainEvents
{
    public interface IDomainEventNotificationFactory
    {
        IDomainEventNotification<IDomainEvent> GetNotification(IDomainEvent domainEvent);
    }
}