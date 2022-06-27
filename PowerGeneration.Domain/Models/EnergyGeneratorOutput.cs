using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGeneration.Domain.Models
{
    public class EnergyGeneratorOutput
    {
        public IEnumerable<GeneratorResult> Totals { get; set; }
        public IEnumerable<EmissionGenerator> MaxEmissionGenerators { get; set; }
        public IEnumerable<Heat> ActualHeatRates { get; set; }
    }
}
