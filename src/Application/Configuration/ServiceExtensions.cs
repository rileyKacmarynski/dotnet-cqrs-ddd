using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SampleStore.Application.Configuration.DomainEvents;
using SampleStore.Application.Configuration.Emails;
using SampleStore.Application.Configuration.Validation;
using SampleStore.Application.Customers.DomainServices;
using SampleStore.Application.Customers.IntegrationHandlers;
using SampleStore.Application.Customers.RegisterCustomer;
using SampleStore.Domain.Customers;
using SampleStore.Domain.Customers.Events;
using SampleStore.Domain.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Configuration
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatr();
            services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);
            services.AddConfigurationSettings<EmailSettings>(configuration);

            services.AddScoped<IUniqueCustomerChecker, UniqueCustomerChecker>();

            return services;
        }

        public static IServiceCollection AddMediatr(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceExtensions).Assembly, typeof(IDomainEvent).Assembly);

            // AddMediatR adds the handlers as IRequestHandler. 
            // It won't add the command handlers we defined as ICommandHandler so we
            // need to do that manually. 
            // We need ICommandHandler so we can add decorators that
            // aren't run on queries
            services.Scan(scan => scan
                .FromAssemblyOf<RegisterCustomerCommandHandler>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces());

            services.Scan(scan => scan
                .FromAssemblyOf<RegisterCustomerCommandHandler>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces());

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));

            return services;
        }

        public static IServiceCollection AddConfigurationSettings<T>(this IServiceCollection services, IConfiguration configuration) where T : class
        {
            services.Configure<T>(options => configuration.GetSection(nameof(T)).Bind(options));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<T>>().Value);

            return services;
        }

    }
}
