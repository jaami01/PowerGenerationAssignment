using PowerGeneration.Domain.Models;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PowerGeneration.Repository.Contracts
{
    public interface IFilePersistance
    {
        Task<XDocument> GenerateXmlDocumentAsync(EnergyGeneratorOutput data);
        bool SaveFile(XDocument document);
        XElement LoadXmlFile(string file);
    }
}