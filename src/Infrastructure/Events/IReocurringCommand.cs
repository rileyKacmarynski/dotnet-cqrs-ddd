using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Events
{
    // marker interface so we can opt out of some decorators for the quartz job (logging)
    public interface IReocurringCommand
    {
    }
}
