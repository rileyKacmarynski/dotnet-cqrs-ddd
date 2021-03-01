using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.SharedKernel.Abstractions
{
    public interface IBusinessRule
    {
        bool IsBroken();
        string Message { get; }
    }
}
