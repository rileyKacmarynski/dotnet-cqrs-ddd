using SampleStore.Domain.SharedKernel;
using SampleStore.Domain.SharedKernel.Abstractions;
using System;

namespace SampleStore.Domain.Payments
{
    public class Payment : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }
        private Guid _orderId;
        private DateTime _createDate;
        private PaymentStatus _status;
        private bool _emailNotificationSent;

        private Payment() { }   // for EF

        public Payment(Guid orderId)
        {
            Id = Guid.NewGuid();
            _createDate = SystemClock.Now;
            _orderId = orderId;
            _status = PaymentStatus.Pending;
            _emailNotificationSent = false;

            this.AddDomainEvent(new PaymentCreatedEvent(Id, _orderId));
        }

        public void MarkEmailNotificationAsSent()
        {
            _emailNotificationSent = true;
        }
    }
}
