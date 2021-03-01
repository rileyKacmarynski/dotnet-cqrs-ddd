using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Domain.SharedKernel.Abstractions
{
    // I don't like this here. Ideally our domain should need to know anything about Task or a unit of work.
    // Should probably try to move it, but I'm assuming there's a reason why it's here in the example I'm going off of. 
    // https://github.com/kgrzybek/sample-dotnet-core-cqrs-api
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
