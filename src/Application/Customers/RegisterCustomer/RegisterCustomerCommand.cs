using SampleStore.Application.Configuration.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Customers.RegisterCustomer
{
    public class RegisterCustomerCommand : CommandBase<CustomerDto>
    {
        public string Email { get; }
        public string Name { get; }

        public RegisterCustomerCommand(string email, string name)
        {
            Email = email;
            Name = name;
        }
    }
}
