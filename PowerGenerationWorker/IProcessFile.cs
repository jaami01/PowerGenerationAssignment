using System.Threading.Tasks;

namespace PowerGenerationWorker
{
    public interface IProcessFile
    {
        Task<bool> ProcessPowerGeneration(string filePath);
    }
}