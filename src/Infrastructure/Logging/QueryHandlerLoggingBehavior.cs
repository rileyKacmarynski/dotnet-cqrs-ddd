using MediatR;
using Microsoft.Extensions.Logging;
using SampleStore.Application.Configuration;
using SampleStore.Application.Configuration.Queries;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Logging
{
    // we can have an IQuery and ICommand logger. The command logger can log the ID of the command as well. 
    internal class QueryHandlerLoggingBehavior<T, TResult> : IPipelineBehavior<T, TResult> where T : IQuery<TResult>
    {
        private readonly ILogger<T> _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;

        public QueryHandlerLoggingBehavior(ILogger<T> logger, IExecutionContextAccessor executionContextAccessor)
        {
            _logger = logger;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<TResult> Handle(T request, CancellationToken cancellationToken, RequestHandlerDelegate<TResult> next)
        {
            using (LogContext.Push(new RequestLogEnricher(_executionContextAccessor)))
            {
                try
                {
                    _logger.LogInformation("Executing query {Query}", request.GetType().Name);

                    var result = await next();

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Query {Query} processing failed", request.GetType().Name);
                    throw;
                }
            }
        }
    }
}
