using MediatR;
using Microsoft.Extensions.Logging;
using SampleStore.Application.Configuration;
using SampleStore.Application.Configuration.Commands;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Logging
{
    internal class CommandHandlerLoggingBehavior<T, TResult> : IPipelineBehavior<T, TResult> where T : ICommand<TResult>
    {
        private readonly ILogger<T> _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;

        public CommandHandlerLoggingBehavior(ILogger<T> logger, IExecutionContextAccessor executionContextAccessor)
        {
            _logger = logger;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            using (LogContext.Push(new RequestLogEnricher(_executionContextAccessor), new CommandLogEnricher(command)))
            {
                try
                {
                    _logger.LogInformation("Executing command {Command}", command.GetType().Name);

                    var result = await next();

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Command {Command} processing failed", command.GetType().Name);
                    throw;
                }
            }
        }

        private class CommandLogEnricher : ILogEventEnricher
        {
            private readonly ICommand<TResult> _command;

            public CommandLogEnricher(ICommand<TResult> command)
            {
                _command = command;
            }
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{_command.Id}")));
            }
        }
    }
}
