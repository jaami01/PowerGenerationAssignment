using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PowerGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Application Starting ");
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var startup = (Startup)Activator.CreateInstance(typeof(Startup), context.Configuration, context.HostingEnvironment);
                    startup.ConfigureServices(services);
                    startup.ConfigureContracts(services);
                })
                .UseSerilog()
                .Build();


            var svc = ActivatorUtilities.CreateInstance<ProcessGenerationData>(host.Services);
            svc.Run();



        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
