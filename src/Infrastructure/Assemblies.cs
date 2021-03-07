using SampleStore.Application.Customers.RegisterCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure
{
    public static class Assemblies
    {
        public static readonly Assembly Application = typeof(RegisterCustomerCommand).Assembly;
    }
}
