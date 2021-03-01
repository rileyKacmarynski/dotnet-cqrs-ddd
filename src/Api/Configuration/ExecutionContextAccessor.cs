using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SampleStore.Application.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Configuration
{
    internal class ExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid CorrelationId
        {
            get
            {
                if (IsAvailable)
                {
                    return Guid.Parse(
                        _httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey]
                    );
                }
                throw new ApplicationException("HttpContext and Correlation ID not available.");
            }
        }

        public bool IsAvailable =>
            _httpContextAccessor.HttpContext != null &&
            _httpContextAccessor.HttpContext.Request.Headers.Keys.Any(x => x == CorrelationMiddleware.CorrelationHeaderKey);
    }

    internal static class ExecutionContextAccessorExtensions
    {
        internal static IServiceCollection AddExecutionContextAccessor(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IExecutionContextAccessor, ExecutionContextAccessor>();

            return services;
        }
    }
}
