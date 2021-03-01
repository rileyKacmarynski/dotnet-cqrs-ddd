using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.SharedKernel.Abstractions
{
    // Domain events are handled in the same transaction as the original event. 
    // A lot of the time there is no domain event handler if the domain event triggers 
    // an asynchronous action. The event is used to create a notification, which gets saved to the
    // outbox and executed later. 
    public class DomainEventBase : IDomainEvent
    {
        public DateTime OccuredOn { get; }
        public DomainEventBase()
        {
            OccuredOn = DateTime.Now;
        }
    }
}
