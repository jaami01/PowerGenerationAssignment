using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerGeneration.Service;
using PowerGenerationConsole;
using System;
using System.Threading.Tasks;


namespace PowerGeneration
{
    public class Startup
    {
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            this._configuration = configuration;
            this._hostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddTransient<IProcessGenerationData, ProcessGenerationData>();
            services.AddTransient<IFolderWatcher,FolderWatcher>();
        }
        public void ConfigureContracts(IServiceCollection services)
        {
            services.AddScoped<IFileProcessService, FileProcessService>();
        }
    }
}
