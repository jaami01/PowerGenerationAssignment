using PowerGeneration.Domain;
using PowerGeneration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PowerGeneration.Repository.Contracts
{
    public interface IFileProcessRepository
    {
        Task<IEnumerable<EnergyGenerator>> ProcessFileAsync(string file);
        IEnumerable<EnergyGenerator> ProcessWindData(IEnumerable<XElement> wind, IList<EnergyGenerator> _energyGenerators);
        IEnumerable<EnergyGeneration> ProcessEnergyGeneration(XElement el);
        IEnumerable<EnergyGenerator> ProcessGasData(IEnumerable<XElement> gas, IList<EnergyGenerator> _energyGenerators);

        IEnumerable<EnergyGenerator> ProcessCoalData(IEnumerable<XElement> coal, IList<EnergyGenerator> _energyGenerators);

    }
}
