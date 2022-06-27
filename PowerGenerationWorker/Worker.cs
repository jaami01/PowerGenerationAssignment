using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PowerGeneration.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PowerGenerationWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHost _host;
        private readonly IOptions<FileName> _fileName;
        private readonly CancellationToken _cancellationToken;

        public Worker(ILogger<Worker> logger,IConfiguration config,IHost host, IOptions<FileName> fileName,CancellationToken cancellationToken = default)
        {
            _logger = logger;
            _configuration = config;
            _host = host;
            this._fileName = fileName;
            _cancellationToken = cancellationToken;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            using var watcher = new FileSystemWatcher(this._fileName.Value.InputFolder);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Error += OnError;

            watcher.Filter = "*.xml";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            return base.StartAsync(cancellationToken);
        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            // this._logger.LogInformation("Changed: "+ e.FullPath);
           // this.StartProcessing();
            
        }
        private void OnCreated(object sender, FileSystemEventArgs e)
        {

            this._logger.LogInformation("Created: " + e.FullPath);
            this.StartProcessing();
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                this._logger.LogInformation($"Message: {ex.Message}");
                this._logger.LogInformation("Stacktrace:");
                this._logger.LogInformation(ex.StackTrace);
                PrintException(ex.InnerException);
            }
        }
        private void StartProcessing()
        {
            try
            {
                var fileName = this._fileName.Value.InputFile;
                if (File.Exists(fileName))
                {
                    using (var serviceScope = _host.Services.CreateScope())
                    {
                        var svc = serviceScope.ServiceProvider.GetRequiredService<IProcessFile>();
                        svc.ProcessPowerGeneration(fileName);
                    }
                }
                else
                {
                    throw new Exception("File does not exist");
                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message, null);
            }
            finally
            {
                //  this.StopAsync(_cancellationToken);
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
