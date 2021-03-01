using SampleStore.Application.Configuration.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Application.Customers.RegisterCustomer
{
    public class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, CustomerDto>
    {
        public Task<CustomerDto> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {


            return Task.FromResult(new CustomerDto { Id = Guid.NewGuid() });
        }
    }
}
