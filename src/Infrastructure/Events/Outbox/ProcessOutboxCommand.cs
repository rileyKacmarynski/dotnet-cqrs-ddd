using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SampleStore.Application.Configuration.Commands;
using SampleStore.Application.Configuration.Data;
using SampleStore.Application.Configuration.DomainEvents;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Events.Outbox
{
    public class ProcessOutboxCommand : CommandBase<Unit>, IReocurringCommand
    {
    }

    public class OutboxCommandHandler : ICommandHandler<ProcessOutboxCommand, Unit>
    {
        private readonly IMediator _mediator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public OutboxCommandHandler(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                   "[OutboxMessage].[Id], " +
                   "[OutboxMessage].[Type], " +
                   "[OutboxMessage].[Data] " +
                   "FROM [app].[OutboxMessages] AS [OutboxMessage] " +
                   "WHERE [OutboxMessage].[ProcessedDate] IS NULL";

            var messages = await connection.QueryAsync<OutboxMessageDto>(sql);
            var messagesList = messages.AsList();
            if (messagesList.Count <= 0)
            {
                return Unit.Value;
            }

            const string sqlUpdateProcessedDate =
                "UPDATE [app].[OutboxMessages] " +
                "SET [ProcessedDate] = @Date " +
                "WHERE [Id] = @Id";

            foreach (var message in messagesList)
            {
                var type = Assemblies.Application
                    .GetType(message.Type);

                var request = JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification;

                using (LogContext.Push(new OutboxMessageContextEnricher(request)))
                {
                    await _mediator.Publish(request, cancellationToken);

                    await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                    {
                        Date = DateTime.UtcNow,
                        message.Id
                    });
                }
            }

            return Unit.Value;
        }

        private class OutboxMessageContextEnricher : ILogEventEnricher
        {
            private readonly IDomainEventNotification _notification;

            public OutboxMessageContextEnricher(IDomainEventNotification notification)
            {
                _notification = notification;
            }
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"OutboxMessage:{_notification.Id.ToString()}")));
            }
        }
    }
}
