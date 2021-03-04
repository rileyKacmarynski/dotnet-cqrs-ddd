using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleStore.Application.Configuration.Commands;
using SampleStore.Application.Configuration.Data;
using SampleStore.Application.Configuration.Queries;
using SampleStore.Infrastructure.Database;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Events
{
    public class UnitOfWorkCommandHandlerWithResultBehavior<T, TResult> : IPipelineBehavior<T, TResult> where T : ICommand<TResult>
    {
        private readonly OrdersContext _ordersContext;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkCommandHandlerWithResultBehavior(OrdersContext ordersContext, IUnitOfWork unitOfWork)
        {
            _ordersContext = ordersContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            var result = await next();



            if (command is InternalCommandBase)
            {
                var internalCommand = await _ordersContext.InternalCommands.FirstOrDefaultAsync(c => c.Id == command.Id);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            

            return result;
        }
    }
}
