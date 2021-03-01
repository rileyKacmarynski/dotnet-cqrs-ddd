using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.SharedKernel.Abstractions
{
     public interface IDomainEvent : INotification
    {
        DateTime OccuredOn { get;  }
    }
}
