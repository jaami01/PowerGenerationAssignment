using PowerGeneration.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerGeneration.Service
{
    public interface IFileProcessService
    {
        Task<bool> ProcessFileAsync(string file);
    }
}