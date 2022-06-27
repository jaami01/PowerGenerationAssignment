using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGenerationConsole
{
    public class FolderWatcher : IFolderWatcher
    {
        public FolderWatcher(ILogger<FolderWatcher> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        private readonly ILogger<FolderWatcher> _log;
        private readonly IConfiguration _config;
        public void Run()
        {
            using var watcher = new FileSystemWatcher(this._config.GetValue<string>("InputFolder"));

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Created += OnCreated;
            watcher.Error += OnError;

            watcher.Filter = "*.xml";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            this._log.LogInformation("Press enter to exit.");
            Console.ReadLine(); // used for console app to behave like service.. TODO => you can use cronejob on az cloud or convert this to windows service
        }
        

        private void OnCreated(object sender, FileSystemEventArgs e)
        {

            string value = $"Created: {e.FullPath}";
            this._log.LogInformation(value);
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
                this._log.LogInformation($"Message: {ex.Message}");
                this._log.LogInformation("Stacktrace:");
                this._log.LogInformation(ex.StackTrace);
                PrintException(ex.InnerException);
            }
        }
    }
}
