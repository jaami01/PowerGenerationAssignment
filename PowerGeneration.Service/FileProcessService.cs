using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PowerGeneration.Domain.Models;
using PowerGeneration.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGeneration.Service
{
    public class FileProcessService : IFileProcessService
    {
        private readonly ILogger<FileProcessService> _log;
        private readonly IFileProcessRepository _repository;
        private readonly IOutputFileProcessRepository _outputRepository;
        private readonly IFilePersistance _filePersistance;
        private readonly IOptions<FileName> _fileName;

        public FileProcessService(ILogger<FileProcessService> log, IFileProcessRepository repository, IOutputFileProcessRepository outputRepository,IFilePersistance filePersistance, IOptions<FileName> fileName)
        {
            this._log = log;
            this._repository = repository;
            this._outputRepository = outputRepository;
            this._filePersistance = filePersistance;
            this._fileName = fileName;
        }
        public async Task<bool> ProcessFileAsync(string file)
        {
            var result = await this._repository.ProcessFileAsync(file).ConfigureAwait(false);
            if (result.Count() > 0)
            {
                this._log.LogInformation("parsing xml file and loading data ....");
                var processedResult = await this._outputRepository.ProcessOutputFileAsync(result).ConfigureAwait(false);
                this._log.LogInformation("creating xml document after processing the data....");
                var document = await this._filePersistance.GenerateXmlDocumentAsync(processedResult);
                if (document != null)
                {
                    this._log.LogInformation("saving xml file to output folder...");
                    var saved = this._filePersistance.SaveFile(document);
                    if (saved)
                    {
                        this._log.LogInformation(saved.ToString());
                        this._log.LogInformation("file saved successfully in output folder");
                        File.Delete(this._fileName.Value.InputFile);
                        this._log.LogInformation("file has been deleted from input folder");
                    }
                    
                }
            }
            return true;
        }
    }
}
