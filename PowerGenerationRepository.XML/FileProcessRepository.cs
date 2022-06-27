using Microsoft.Extensions.Logging;
using PowerGeneration.Common;
using PowerGeneration.Domain;
using PowerGeneration.Domain.Models;
using PowerGeneration.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PowerGenerationRepository.XML
{
    public class FileProcessRepository : IFileProcessRepository
    {
        private readonly ILogger<FileProcessRepository> _log;
        private readonly IFilePersistance _filePersistance;
        private IList<EnergyGenerator> _energyGenerators = new List<EnergyGenerator>();
        public FileProcessRepository(ILogger<FileProcessRepository> log,IFilePersistance filePersistance)
        {
            this._log = log;
            this._filePersistance = filePersistance;
        }
        public async Task<IEnumerable<EnergyGenerator>> ProcessFileAsync(string file)
        {
            if(string.IsNullOrEmpty(file)) throw new ArgumentNullException(nameof(file));
            try
            {

                XElement root = this._filePersistance.LoadXmlFile(file);
                IEnumerable<XElement> wind = root.Descendants("Wind");
                IEnumerable<XElement> gas = root.Descendants("Gas");
                IEnumerable<XElement> coal = root.Descendants("Coal");
                this.ProcessWindData(wind,this._energyGenerators);
                this.ProcessGasData(gas, this._energyGenerators);
                this.ProcessCoalData(coal, this._energyGenerators);
                return _energyGenerators;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
        public IEnumerable<EnergyGenerator> ProcessWindData(IEnumerable<XElement> wind, IList<EnergyGenerator> _energyGenerators)
        {
            if(_energyGenerators == null) throw new ArgumentNullException(nameof(_energyGenerators));
            if(wind == null) this._log.LogInformation("file doesnt have any wind data");
            if(!wind.Any()) this._log.LogInformation("wind element is empty in file");
            IEnumerable<XElement> windGenerators = wind.Descendants("WindGenerator");
            if(!windGenerators.Any()) this._log.LogInformation("there is no data under wind element in file");
            try
            {
                EnergyGenerator _energyGenerator = new EnergyGenerator();
                foreach (XElement el in windGenerators)
                {
                    IEnumerable<XElement> name = el.Descendants("Name");
                    IEnumerable<XElement> location = el.Descendants("Location");
                    _energyGenerator = new EnergyGenerator()
                    {
                        Name = name.FirstOrDefault()!=null ? name.FirstOrDefault().Value:"",
                        Location = location.FirstOrDefault()!=null? location.FirstOrDefault().Value:"",
                        Type = EnergyGenerator.GeneratorType.Wind
                    };

                    var energyGenerations = ProcessEnergyGeneration(el);
                    _energyGenerator.energyGenerations = energyGenerations;
                    _energyGenerators.Add(_energyGenerator);
                }
                return _energyGenerators;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public IEnumerable<EnergyGeneration> ProcessEnergyGeneration(XElement el)
        {
            // if (_energyGenerator == null) throw new ArgumentNullException(nameof(_energyGenerator));
            if(el == null) throw new ArgumentNullException(nameof(el));
            if(!el.HasElements) throw new ArgumentException(nameof(el));
            IEnumerable<XElement> Generation = el.Descendants("Generation");
            IList<EnergyGeneration> _energyGenerations = new List<EnergyGeneration>(); 
            // if (!Generation.Any()) return Task.FromResult(false);
            try
            {
                foreach (XElement gen in Generation)
                {
                    _energyGenerations = new List<EnergyGeneration>();
                    IEnumerable<XElement> Day = gen.Descendants("Day");
                    foreach (XElement day in Day)
                    {
                        IEnumerable<XElement> date = day.Descendants("Date");
                        IEnumerable<XElement> energy = day.Descendants("Energy");
                        IEnumerable<XElement> price = day.Descendants("Price");
                        _energyGenerations.Add(new EnergyGeneration()
                        {
                            Day = date.FirstOrDefault() != null ? date.FirstOrDefault().Value.ValidOrDefaultDate() : DateTime.MinValue,
                            Energy = energy.FirstOrDefault() != null ? energy.FirstOrDefault().Value.ValidOrDefaultDecimal() : 0,
                            Price = price.FirstOrDefault() != null ? price.FirstOrDefault().Value.ValidOrDefaultDecimal() : 0,
                        });

                    }
                  //  _energyGenerator.energyGenerations = _energyGenerations;
                }
                return _energyGenerations;
;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public IEnumerable<EnergyGenerator> ProcessGasData(IEnumerable<XElement> gas, IList<EnergyGenerator> _energyGenerators)
        {
            if (gas == null) this._log.LogInformation("file doesnt have any gas data");
            if (!gas.Any()) this._log.LogInformation("gas element is empty in file");
            IEnumerable<XElement> gasGenerators = gas.Descendants("GasGenerator");
            if(gasGenerators == null) this._log.LogInformation("there is no data under gas element in file"); 
            try
            {
                var _energyGenerator = new EnergyGenerator();
                foreach (XElement el in gasGenerators)
                {
                    IEnumerable<XElement> name = el.Descendants("Name");
                    IEnumerable<XElement> emissionsRating = el.Descendants("EmissionsRating");
                    _energyGenerator = new EnergyGenerator()
                    {
                        Name = name.FirstOrDefault() != null ? name.FirstOrDefault().Value : "",
                        EmissionsRating = emissionsRating.FirstOrDefault() != null ? emissionsRating.FirstOrDefault().Value.ValidOrDefaultDecimal() : 0,
                        Type = EnergyGenerator.GeneratorType.Gas
                    };

                    var energyGenerations = ProcessEnergyGeneration(el);
                    _energyGenerator.energyGenerations = energyGenerations;
                    _energyGenerators.Add(_energyGenerator);
                }
                return _energyGenerators;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public IEnumerable<EnergyGenerator> ProcessCoalData(IEnumerable<XElement> coal, IList<EnergyGenerator> _energyGenerators)
        {
            if (coal == null) this._log.LogInformation("file doesnt have any coal data");
            if (!coal.Any()) this._log.LogInformation("coal element is empty in file");
            IEnumerable<XElement> coalGenerators = coal.Descendants("CoalGenerator");
            if(coalGenerators == null) this._log.LogInformation("there is no data under coal element in file"); ;
            try
            {
                var _energyGenerator = new EnergyGenerator();
                foreach(XElement el in coalGenerators)
                {
                    IEnumerable<XElement> name = el.Descendants("Name");
                    IEnumerable<XElement> totalHeatInput = el.Descendants("TotalHeatInput");
                    IEnumerable<XElement> actualNetGeneration = el.Descendants("ActualNetGeneration");
                    IEnumerable<XElement> emissionsRating = el.Descendants("EmissionsRating");
                    _energyGenerator = new EnergyGenerator()
                    {
                        Name = name.FirstOrDefault() != null ? name.FirstOrDefault().Value : "",
                        TotalHeatInput = totalHeatInput.FirstOrDefault() != null ? totalHeatInput.FirstOrDefault().Value.ValidOrDefaultDecimal() : 0,
                        ActualNetGeneration = actualNetGeneration.FirstOrDefault() != null ? actualNetGeneration.FirstOrDefault().Value.ValidOrDefaultDecimal() : 0,
                        EmissionsRating = emissionsRating.FirstOrDefault() != null ? emissionsRating.FirstOrDefault().Value.ValidOrDefaultDecimal() : 0,
                        Type = EnergyGenerator.GeneratorType.Coal
                    };

                    var energyGenerations = ProcessEnergyGeneration(el);
                    _energyGenerator.energyGenerations = energyGenerations;
                    _energyGenerators.Add(_energyGenerator);
                }
                return _energyGenerators;
            }
            catch (Exception)
            {

                throw;
            }
          
        }
      
    }
}
