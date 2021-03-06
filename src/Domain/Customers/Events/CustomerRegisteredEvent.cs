﻿using SampleStore.Domain.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.Customers.Events
{
    public class CustomerRegisteredEvent : DomainEventBase
    {
        public CustomerId CustomerId { get;  }
        public CustomerRegisteredEvent(CustomerId customerId)
        {
            CustomerId = customerId;
        }
    }
}
