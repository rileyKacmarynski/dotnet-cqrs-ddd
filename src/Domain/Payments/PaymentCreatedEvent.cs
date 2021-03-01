using SampleStore.Domain.SharedKernel.Abstractions;
using System;

namespace SampleStore.Domain.Payments
{
    internal class PaymentCreatedEvent : DomainEventBase
    {
        private Guid _id;
        private Guid _orderId;

        public PaymentCreatedEvent(Guid id, Guid orderId)
        {
            _id = id;
            _orderId = orderId;
        }
    }
}