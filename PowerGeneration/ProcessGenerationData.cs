using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PowerGeneration
{
    public class ProcessGenerationData : IProcessGenerationData
    {
        public ProcessGenerationData(ILogger<ProcessGenerationData> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        private readonly ILogger<ProcessGenerationData> _log;
        private readonly IConfiguration _config;

      public  void Run()
        {
            for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
            {
                _log.LogInformation("Run Number {runNumber}", i);
            }
        }


    }
}
