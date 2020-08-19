using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XMemes.Services
{
    public class App
    {
        private readonly ILogger<App> _log;

        public App(ILogger<App> log)
        {
            _log = log;
        }

        public async Task Run()
        {
            _log.LogInformation("Application Starting...");
            await Task.Delay(10);
            _log.LogInformation("Application Shutting Down...");
        }
    }
}