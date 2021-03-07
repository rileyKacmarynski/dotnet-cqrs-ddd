using SampleStore.Domain.SharedKernel.Abstractions;
using SampleStore.Domain.SharedKernel.ValueObjects;
using System;

namespace SampleStore.Domain.Customers
{
    public class OrderPlacedEvent : DomainEventBase
    {
        
        public OrderPlacedEvent(Guid orderId, CustomerId customerId, MoneyValue total)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Total = total;
        }

        public Guid OrderId { get; }
        public CustomerId CustomerId { get; }
        public MoneyValue Total { get; }
    }
}