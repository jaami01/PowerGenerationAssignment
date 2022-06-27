using PowerGeneration.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerGeneration.Repository.Contracts
{
    public interface IOutputFileProcessRepository
    {
        Task<EnergyGeneratorOutput> ProcessOutputFileAsync(IEnumerable<EnergyGenerator> energyGenerators);
    }
}