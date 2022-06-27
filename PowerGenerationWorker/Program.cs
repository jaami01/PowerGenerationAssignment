using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerGenerationWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var startup = (Startup)Activator.CreateInstance(typeof(Startup), hostContext.Configuration, hostContext.HostingEnvironment);
                    startup.ConfigureServices(services);
                    startup.ConfigureContracts(services);
                    
                });
    }
}
