using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace XMemes.Services
{
    public static class Program
    {
        public static async Task Main(string[] _)
        {
            var host =
                Host.CreateDefaultBuilder()
                    .ConfigureServices((ctx, services) =>
                    {
                        services.ConfigureServices();
                    })
                    .Build();

            var app = ActivatorUtilities.CreateInstance<App>(host.Services);
            await app.Run();
        }
    }
}
