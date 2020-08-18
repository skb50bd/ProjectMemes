using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace XMemes.Services
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var log = ServiceConfiguration.CreateLogger();
            log.Information("Application Starting...");

            var host = Host.CreateDefaultBuilder().Build();
        }
    }
}
