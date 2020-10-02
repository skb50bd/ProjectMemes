using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using XMemes.Data.Repositories;
using XMemes.Models.Domain;
using Tag = XMemes.Models.Domain.Tag;

namespace XMemes.Data
{
    public static class DataConfiguration
    {
        public static IServiceCollection ConfigureData(
            this IServiceCollection services   ,
            IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            services.AddSingleton<IMongoClient>(client);

            services.AddTransient<IMemeRepository, MongoMemeRepository>();
            services.AddTransient<IMemerRepository, MongoMemerRepository>();
            services.AddTransient<IRepository<Tag>, MongoTagRepository>();
            services.AddTransient<IRepository<Template>, MongoTemplateRepository>();
            services.AddTransient<IFileRepository, AzureBlobFileRepository>();

            return services;
        }
    }
}
