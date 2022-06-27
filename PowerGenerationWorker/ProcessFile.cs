using Microsoft.Extensions.Logging;
using PowerGeneration.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerGenerationWorker
{
    public class ProcessFile : IProcessFile
    {
        private readonly ILogger<ProcessFile> _logger;
        private readonly IFileProcessService _fileProcessService;

        public ProcessFile(ILogger<ProcessFile> logger,IFileProcessService fileProcessService)
        {
            this._logger = logger;
            this._fileProcessService = fileProcessService;
        }
        public async Task<bool> ProcessPowerGeneration(string filePath)
        {
            this._logger.LogInformation("file processing started....");
           var result = await this._fileProcessService.ProcessFileAsync(filePath).ConfigureAwait(false);
           return result;
        }
    }
}
