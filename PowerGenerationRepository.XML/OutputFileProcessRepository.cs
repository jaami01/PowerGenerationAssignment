using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PowerGeneration.Domain.Models;
using PowerGeneration.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGenerationRepository.XML
{
    public class OutputFileProcessRepository : IOutputFileProcessRepository
    {
        private readonly ILogger<OutputFileProcessRepository> _log;
        private readonly IConfiguration _config;
        private readonly IOptions<ValueFactor> _valueFactor;
        private readonly IOptions<EmissionsFactor> _emisstionsFactor;
        private readonly EnergyGeneratorOutput _generatorsOutput;

        public OutputFileProcessRepository(ILogger<OutputFileProcessRepository> log, IConfiguration config, IOptions<ValueFactor> valueFactor, IOptions<EmissionsFactor> emisstionsFactor)
        {
            this._log = log;
            this._config = config;
            this._valueFactor = valueFactor;
            this._emisstionsFactor = emisstionsFactor;
            this._generatorsOutput = new EnergyGeneratorOutput();
        }

        public async Task<EnergyGeneratorOutput> ProcessOutputFileAsync(IEnumerable<EnergyGenerator> energyGenerators)
        {
            if(energyGenerators == null) throw new ArgumentNullException(nameof(energyGenerators));
            if (!energyGenerators.Any()) throw new ArgumentException("file is empty");
            this._log.LogInformation("file is empty");
            IList<GeneratorResult> totals = new List<GeneratorResult>();
            IList<EmissionGenerator> emissionGenerators = new List<EmissionGenerator>();
            IList<Heat> actualHeatRates = new List<Heat>();
            foreach (var item in energyGenerators)
            {
                if (item.Type == EnergyGenerator.GeneratorType.Wind && item.Location == "Offshore")
                {
                    totals.Add(new GeneratorResult()
                    {
                        Name = item.Name,
                        Total = item.energyGenerations.Sum(x => x.Energy * x.Price * this._valueFactor.Value.Low)
                    });
                }
                else if (item.Type == EnergyGenerator.GeneratorType.Wind && item.Location == "Onshore")
                {
                    totals.Add(new GeneratorResult()
                    {
                        Name = item.Name,
                        Total = item.energyGenerations.Sum(x => x.Energy * x.Price * this._valueFactor.Value.High)
                    });
                }
                else
                {
                    totals.Add(new GeneratorResult()
                    {
                        Name = item.Name,
                        Total = item.energyGenerations.Sum(x => x.Energy * x.Price * this._valueFactor.Value.Medium)
                    });
                    if(item.Type == EnergyGenerator.GeneratorType.Coal)
                    {
                        actualHeatRates.Add(new Heat
                        {
                            Name = item.Name,
                            HeatRate = (int)(item.TotalHeatInput / item.ActualNetGeneration)
                        });
                        foreach (var energyItem in item.energyGenerations)
                        {
                            emissionGenerators.Add(new EmissionGenerator()
                            {
                                Date = energyItem.Day,
                                Name = item.Name,
                                Emission = energyItem.Energy * this._emisstionsFactor.Value.High * item.EmissionsRating,
                            });
                        }
                    }
                    else if(item.Type == EnergyGenerator.GeneratorType.Gas)
                    {
                        foreach (var energyItem in item.energyGenerations)
                        {
                            emissionGenerators.Add(new EmissionGenerator()
                            {
                                Date = energyItem.Day,
                                Name = item.Name,
                                Emission = energyItem.Energy * this._emisstionsFactor.Value.Medium * item.EmissionsRating,
                            });
                        }
                    }
                }

            }
            this._generatorsOutput.Totals = totals;
            this._generatorsOutput.MaxEmissionGenerators = emissionGenerators;
            this._generatorsOutput.ActualHeatRates = actualHeatRates;
            return this._generatorsOutput;
        }


    }
}
