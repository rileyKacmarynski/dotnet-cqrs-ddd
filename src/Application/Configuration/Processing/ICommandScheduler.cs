using SampleStore.Application.Configuration.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Configuration.Processing
{
    public interface ICommandScheduler
    {
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}
