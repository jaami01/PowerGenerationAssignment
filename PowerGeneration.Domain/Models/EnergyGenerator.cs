using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGeneration.Domain.Models
{
    public class EnergyGenerator
    {
        public string Name { get; set; }
        public GeneratorType Type { get; set; }
        public string Location { get; set; }
        public decimal EmissionsRating { get; set; }
        public decimal TotalHeatInput { get; set; }
        public decimal ActualNetGeneration { get; set; }

        public IEnumerable<EnergyGeneration> energyGenerations { get; set; }

        public enum GeneratorType
        {
            Wind = 1, Gas,Coal
        }
    }
}
