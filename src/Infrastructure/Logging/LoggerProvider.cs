using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;

namespace SampleStore.Infrastructure.Logging
{
    public class LoggerProvider
    {
        // The configuration could be used to build the logger. 
        public static ILogger CreateLogger(IConfiguration configuration)
        {

            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("App Name", "Serilog Web App Sample")
                .CreateLogger();
        }

        public static ILogger CreateLogger(string appName)
        {
            return new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("App Name", appName)
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
                    .WriteTo.File(new CompactJsonFormatter(), "logs/logs", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
        }
    }
}
