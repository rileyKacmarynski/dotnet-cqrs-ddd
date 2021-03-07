using SampleStore.Application.Configuration.Commands;
using SampleStore.Domain.Customers;
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
        private readonly ICustomerRepository _customerRepository;
        private readonly IUniqueCustomerChecker _uniqueCustomerChecker;

        public RegisterCustomerCommandHandler(ICustomerRepository customerRepository, IUniqueCustomerChecker uniqueCustomerChecker)
        {
            _customerRepository = customerRepository;
            _uniqueCustomerChecker = uniqueCustomerChecker;
        }

        public async Task<CustomerDto> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = Customer.Register(request.Email, request.Name, _uniqueCustomerChecker);

            await _customerRepository.AddAsync(customer);

            return new CustomerDto { Id = customer.Id.Value };
        }
    }
}
