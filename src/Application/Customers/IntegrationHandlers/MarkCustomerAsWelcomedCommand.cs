using MediatR;
using SampleStore.Application.Configuration.Commands;
using SampleStore.Domain.Customers;
using System;

namespace SampleStore.Application.Customers.IntegrationHandlers
{
    public class MarkCustomerAsWelcomedCommand : InternalCommandBase<Unit>
    {
        public MarkCustomerAsWelcomedCommand(Guid id, CustomerId customerId) : base(id)
        {
            CustomerId = customerId;
        }

        public CustomerId CustomerId { get; }
    }
}