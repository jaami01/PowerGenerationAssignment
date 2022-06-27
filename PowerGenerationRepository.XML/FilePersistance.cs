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
using System.Xml.Linq;

namespace PowerGenerationRepository.XML
{
    public class FilePersistance : IFilePersistance
    {
        private readonly ILogger<OutputFileProcessRepository> _log;
        private readonly IConfiguration _config;
        private readonly IOptions<FileName> _fileName;

        public FilePersistance(ILogger<OutputFileProcessRepository> log, IConfiguration config, IOptions<FileName> fileName)
        {
            this._log = log;
            this._config = config;
            this._fileName = fileName;
        }
        public XElement LoadXmlFile(string file)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentNullException();
            try
            {
                XElement xml = XElement.Load(file);
                if (xml == null) throw new ArgumentNullException(nameof(xml));
                if (xml.IsEmpty) throw new ArgumentException("file is empty");

                return xml;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<XDocument> GenerateXmlDocumentAsync(EnergyGeneratorOutput data)
        {
            var document = new XDocument(
                new XElement("GenerationOutput",
                    new XElement("Totals",
                         from total in data.Totals
                         select new XElement("Generator", new XElement("Name", total.Name),
                                                          new XElement("Total", total.Total))),
                    new XElement("MaxEmissionGenerators",
                        from emission in data.MaxEmissionGenerators
                        select new XElement("Day", new XElement("Name", emission.Name),
                                                   new XElement("Date", emission.Date),
                                                   new XElement("Emission", emission.Emission))),
                    new XElement("ActualHeatRates",
                         from heat in data.ActualHeatRates
                         select new XElement("ActualHeatRate", new XElement("Name", heat.Name),
                                                          new XElement("HeatRate", heat.HeatRate)))));
            return document;
        }
        public bool SaveFile(XDocument document)
        {
            if(document == null) throw new ArgumentNullException(nameof(document));
            try
            {
                document.Save(this._fileName.Value.OutputFile);
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }
    }
}
