using MediatR;
using SampleStore.Application.Configuration.Commands;
using System;

namespace SampleStore.Application.Customers.IntegrationHandlers
{
    public class MarkCustomerAsWelcomedCommand : InternalCommandBase<Unit>
    {
        public MarkCustomerAsWelcomedCommand(Guid id, Guid customerId) : base(id)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
    }
}