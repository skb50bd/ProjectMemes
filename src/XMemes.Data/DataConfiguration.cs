using Microsoft.Extensions.DependencyInjection;
using XMemes.Data.Repositories;
using XMemes.Models.Domain;

namespace XMemes.Data
{
    public static class DataConfiguration
    {
        public static IServiceCollection ConfigureData(this IServiceCollection services)
        {
            services.AddTransient<IMemeRepository, MemeRepository>();
            services.AddTransient<IRepository<Memer>, Repository<Memer>>();
            services.AddTransient<IRepository<Tag>, Repository<Tag>>();
            services.AddTransient<IRepository<Template>, Repository<Template>>();

            return services;
        }
    }
}
