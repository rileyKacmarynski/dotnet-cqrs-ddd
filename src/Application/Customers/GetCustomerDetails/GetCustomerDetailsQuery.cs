using SampleStore.Application.Configuration.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleStore.Application.Customers.GetCustomerDetails
{
    public class GetCustomerDetailsQuery : IQuery<CustomerDetailsDto>
    {
        public GetCustomerDetailsQuery(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
    }
}
