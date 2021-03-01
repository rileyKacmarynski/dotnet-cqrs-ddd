using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.Customers
{
    public interface IUniqueCustomerChecker
    {
        public bool IsUnique(string email);
    }
}
