using Dapper;
using MediatR;
using Quartz;
using SampleStore.Application.Configuration.Data;
using SampleStore.Application.Configuration.StronglyTypedIds;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Events.InternalCommands
{
    [DisallowConcurrentExecution]
    public class ProcessInternalCommandsJob : IJob
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IMediator _mediator;

        public ProcessInternalCommandsJob(ISqlConnectionFactory sqlConnectionFactory, IMediator mediator)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT " +
                   "[Command].[Type], " +
                   "[Command].[Data] " +
                   "FROM [app].[InternalCommands] AS [Command] " +
                   "WHERE [Command].[ProcessedDate] IS NULL";

            var commands = await connection.QueryAsync<InternalCommandDto>(sql);
            var internalCommandsList = commands.AsList();

            foreach(var internalCommand in internalCommandsList)
            {
                var type = Assemblies.Application.GetType(internalCommand.Type);

                var options = new JsonSerializerOptions();
                options.Converters.Add(new StronglyTypedIdJsonConverterFactory());
                dynamic commandToProcess = JsonSerializer.Deserialize(internalCommand.Data, type, options);

                await _mediator.Send(commandToProcess, context.CancellationToken);

                const string insertSql =
                    "UPDATE [app].[InternalCommands] " +
                    "SET [ProcessedDate] = @ProcessedDate " +
                    "WHERE [Id] = @Id";

                await connection.ExecuteAsync(insertSql, new
                {
                    Id = commandToProcess.Id,
                    ProcessedDate = DateTime.UtcNow
                });
            }
        }
    }
}
