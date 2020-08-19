using System;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using XMemes.Data;

namespace XMemes.Services
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureServices(
            this IServiceCollection services)
        {
            var log = CreateLogger();
            services.AddLogging(builder => builder.AddSerilog(log));

            services.ConfigureData();

            // Add Services Here
            services.AddSingleton<App>();

            return services;
        }

        public static ILogger CreateLogger() =>
            new LoggerConfiguration()
                .ReadFrom.Configuration(
                    new ConfigurationBuilder()
                        .BuildLoggerConfiguration()
                        .Build())
                .CreateLogger();

        private static string HostingEnvironment =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") 
            ?? "Production";

        private static IConfigurationBuilder BuildLoggerConfiguration(
            this IConfigurationBuilder builder) =>
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{HostingEnvironment}.json", optional: true)
                .AddEnvironmentVariables();
    }
}
