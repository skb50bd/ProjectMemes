using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace XMemes.Services
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            var log = CreateLogger();
            services.AddLogging(builder => builder.AddSerilog(log));
         
            return services;
        }

        public static ILogger CreateLogger()
        {
            var builder = new ConfigurationBuilder();
            builder.BuildLoggerConfiguration();

            Log.Logger =
                new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Build())
                    .CreateLogger();

            return Log.Logger;
        }

        private static IConfigurationBuilder BuildLoggerConfiguration(
            this IConfigurationBuilder builder)
        {
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    optional: true)
                .AddEnvironmentVariables();

            return builder;
        }
    }
}
