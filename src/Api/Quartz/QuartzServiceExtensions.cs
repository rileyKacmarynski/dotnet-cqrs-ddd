using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace SampleStore.Api.Quartz
{
    public static class QuartzServiceExtensions
    {
        public static IServiceCollection AddQuartz(this IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<QuartzJobRunner>();
            services.AddHostedService<QuartzHostedService>();

            return services;
        }

        public static IServiceCollection AddQuartzJob<T>(this IServiceCollection services, string cronExpression)
            where T : IJob
        {
            services.AddScoped(typeof(T));
            services.AddSingleton(new JobSchedule(
                jobType: typeof(T),
                cronExpression: cronExpression));

            return services;
        }
    }
}
