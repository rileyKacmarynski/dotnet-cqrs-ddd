using MediatR;
using Microsoft.EntityFrameworkCore;
using SampleStore.Application.Configuration.Commands;
using SampleStore.Application.Configuration.Data;
using SampleStore.Infrastructure.Database;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Events
{
    public class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : ICommand
    {
        private readonly ICommandHandler<T> _decorated;
        private readonly OrdersContext _ordersContext;
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkCommandHandlerDecorator(ICommandHandler<T> decorated, OrdersContext ordersContext, IUnitOfWork unitOfWork)
        {
            _decorated = decorated;
            _ordersContext = ordersContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(T command, CancellationToken cancellationToken)
        {
            await _decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase)
            {
                var internalCommand = await _ordersContext.InternalCommands.FirstOrDefaultAsync(c => c.Id == command.Id);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
