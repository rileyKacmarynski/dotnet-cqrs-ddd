using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleStore.Application.Configuration.Data;
using SampleStore.Application.Configuration.DomainEvents;
using SampleStore.Application.Configuration.Emails;
using SampleStore.Application.Configuration.Processing;
using SampleStore.Application.Customers;
using SampleStore.Application.Payments;
using SampleStore.Application.Products;
using SampleStore.Domain;
using SampleStore.Infrastructure.Database;
using SampleStore.Infrastructure.Domain.Customers;
using SampleStore.Infrastructure.Domain.Payments;
using SampleStore.Infrastructure.Domain.Products;
using SampleStore.Infrastructure.Emails;
using SampleStore.Infrastructure.Events;
using SampleStore.Infrastructure.Logging;
using System;

namespace SampleStore.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // I don't really like the names of these methods.
            // AddCommands just set up logging and unit of work decorators
            return services
                .AddData(configuration)
                .AddEvents()
                .AddCommands()
                .AddEmail();
        }

        private static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("OrdersDatabase");
            services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            services.AddDbContext<OrdersContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            return services;
        }

        private static IServiceCollection AddEvents(this IServiceCollection services)
        {
            services.AddSingleton<IDomainEventNotificationFactory, DomainEventNotificationFactory>();
            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            services.TryDecorate(
                typeof(INotificationHandler<>),
                typeof(DomainEventsDispatcherNotificationHandlerDecorator<>));

            return services;
        }

        private static IServiceCollection AddCommands(this IServiceCollection services)
        {
            // This is a repeat of what's on the application service extensions.
            // this registeres the decorators in the infrastructure project whereas
            // the code in the application service extensions registers the actual
            // command and queries. Not sure how I feel about the duplication of logic though. 
            services.Scan(scan => scan
                .FromAssemblyOf<DomainEventsDispatcher>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces());


            services.Scan(scan => scan
                .FromAssemblyOf<DomainEventsDispatcher>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces());

            services.AddPipelineBehavior(typeof(QueryHandlerLoggingBehavior<,>));

            services.AddPipelineBehavior(typeof(CommandHandlerLoggingBehavior<,>));
            services.AddPipelineBehavior(typeof(UnitOfWorkCommandHandlerWithResultBehavior<,>));

            services.AddScoped<ICommandScheduler, CommandScheduler>();

            return services;
        }

        private static IServiceCollection AddEmail(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSender>();

            return services;
        }

        private static IServiceCollection AddPipelineBehavior(this IServiceCollection services, Type t)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), t);
            return services;
        }
    }
}
