using SampleStore.Domain.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.Customers.Rules
{
    public class CustomerEmailMustBeUniqueRule : IBusinessRule
    {
        private readonly string _email;
        private readonly IUniqueCustomerChecker _uniqueCustomerChecker;

        public CustomerEmailMustBeUniqueRule(string email, IUniqueCustomerChecker uniqueCustomerChecker)
        {
            _email = email;
            _uniqueCustomerChecker = uniqueCustomerChecker;
        }
        public string Message => "Customer with email already exists.";

        public bool IsBroken() => !_uniqueCustomerChecker.IsUnique(_email);
    }
}
