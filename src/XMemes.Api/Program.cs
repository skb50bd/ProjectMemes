using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;

using XMemes.Services;

namespace XMemes.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var logger = ServiceConfiguration.CreateLogger();
            return Host.CreateDefaultBuilder(args)
                       .UseSerilog(logger)
                       .ConfigureWebHostDefaults(
                           webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}
