using SampleStore.Application.Configuration.Data;
using SampleStore.Infrastructure.Database;
using SampleStore.Infrastructure.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrdersContext _ordersContext;
        private readonly IDomainEventsDispatcher _domainEventDispatcher;

        public UnitOfWork(OrdersContext ordersContext, IDomainEventsDispatcher domainEventDispatcher)
        {
            _ordersContext = ordersContext;
            _domainEventDispatcher = domainEventDispatcher;
        }
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            await _domainEventDispatcher.DispatchEventAsync();
            return await _ordersContext.SaveChangesAsync();
        }
    }
}
