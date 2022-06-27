using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PowerGeneration.Domain.Models;
using PowerGeneration.Repository.Contracts;
using PowerGeneration.Service;
using PowerGenerationRepository.XML;
using PowerGenerationWorker;
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
            services.AddHostedService<Worker>();
            services.AddScoped<IProcessFile, ProcessFile>();

        }
        public void ConfigureContracts(IServiceCollection services)
        {
            services.AddScoped<IFileProcessService, FileProcessService>();
            services.AddScoped<IFileProcessRepository, FileProcessRepository>();
            services.AddScoped<IOutputFileProcessRepository, OutputFileProcessRepository>();
            services.AddScoped<IFilePersistance, FilePersistance>();
            services.Configure<ValueFactor>(_configuration.GetSection("ValueFactor"));
            services.Configure<EmissionsFactor>(_configuration.GetSection("EmissionsFactor"));
            services.Configure<FileName>(_configuration.GetSection("FilesPath"));
        }
    }
}
