using System;
using System.IO;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using XMemes.Data;
using XMemes.Models;
using XMemes.Models.InputModels;
using XMemes.Models.ViewModels;
using XMemes.Services.Abstractions;
using XMemes.Services.Implementations;

namespace XMemes.Services
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            IConfiguration config)
        {
            var log = CreateLogger();
            services.AddLogging(builder => builder.AddSerilog(log));

            var conf = (Action<IMapperConfigurationExpression>) MappingConfig.Config;
            services.AddAutoMapper(conf);

            services.ConfigureData(config);

            services.AddTransient<IService<TagViewModel, TagInput>, TagService>();
            services.AddTransient<IMemerService, MemerService>();
            services.AddTransient<IMemeService, MemeService>();

            services.AddTransient<IImageService, ImageService>();

            services.Configure<Settings>(config.GetSection(nameof(Settings)));
            
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
