using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleStore.Application.Configuration.Validation;
using SampleStore.Application.Customers.RegisterCustomer;
using SampleStore.Domain.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Configuration
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureMediatr();
            services.AddValidatorsFromAssembly(typeof(ServicesExtensions).Assembly);

            return services;
        }

        public static IServiceCollection ConfigureMediatr(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServicesExtensions).Assembly, typeof(IDomainEvent).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));

            return services;
        }
    }
}
