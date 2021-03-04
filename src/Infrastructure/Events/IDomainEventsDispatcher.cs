using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Events
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventAsync();
    }
}