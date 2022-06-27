using Moq;
using PowerGeneration.Repository.Contracts;
using PowerGenerationRepository.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace PowerGeneration.Repository.Xml.Tests
{
    public class FileProcessRepositoryTest
    {
        private readonly IFileProcessRepository _fileProcessRepository;
        public FileProcessRepositoryTest()
        {
            this._fileProcessRepository = new FileProcessRepository(default,default);
        }
        
        [Fact]
        public async void ProcessFileAsync_ShouldThrowException_IfFileNameIsNull()
        {
           await Assert.ThrowsAsync<ArgumentNullException>(async () => await this._fileProcessRepository.ProcessFileAsync(null).ConfigureAwait(false));
        }
        [Fact]
        public async void ProcessFileAsync_ShouldThrowException_IfFileNameIsEmpty()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await this._fileProcessRepository.ProcessFileAsync("").ConfigureAwait(false));
        }
        [Fact]
        public void ProcessWindData_ShouldThrowException_IfReferenceObjectIsNull()
        {

            //arrange
            XElement obj = new XElement("wind",
                new XElement("Generators",
                new XElement("test")));
            List<XElement> wind = new List<XElement>();
            wind.Add(obj);
            
            
            // assert
            Assert.Throws<ArgumentNullException>(() => this._fileProcessRepository.ProcessWindData(wind, null));
        }
    }
}
