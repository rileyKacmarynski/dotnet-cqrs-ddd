using System;

namespace SampleStore.Application.Customers.GetCustomerDetails
{
    public class CustomerDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool WelcomeEmailWasSent { get; set; }
    }
}
