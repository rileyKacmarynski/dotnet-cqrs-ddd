using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;
using SampleStore.Application.Configuration.Data;
using SampleStore.Application.Configuration.DomainEvents;
using SampleStore.Application.Configuration.StronglyTypedIds;
using SampleStore.Infrastructure;
using Serilog.Context;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Events.Outbox
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ProcessOutboxJob(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task Execute(IJobExecutionContext context)
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
                return;
            }

            const string sqlUpdateProcessedDate =
                "UPDATE [app].[OutboxMessages] " +
                "SET [ProcessedDate] = @Date " +
                "WHERE [Id] = @Id";

            foreach (var message in messagesList)
            {
                var type = Assemblies.Application
                    .GetType(message.Type);

                var options = new JsonSerializerOptions();
                options.Converters.Add(new StronglyTypedIdJsonConverterFactory());
                var request = JsonSerializer.Deserialize(message.Data, type, options) as IDomainEventNotification;

                await _mediator.Publish(request, context.CancellationToken);

                await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                {
                    Date = DateTime.UtcNow,
                    message.Id
                });
            }
        }
    }
}
