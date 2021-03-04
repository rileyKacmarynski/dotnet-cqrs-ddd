using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleStore.Api
{
    public class RegisterCustomerRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
