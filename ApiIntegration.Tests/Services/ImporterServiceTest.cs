using System.Threading.Tasks;
using ApiIntegration.Interfaces;
using ApiIntegration.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ApiIntegration.Tests.Services
{
    [TestClass]
    public class ImporterServiceTest
    {
        private readonly IImporterService _importerService;

        public ImporterServiceTest()
            => _importerService = new ImporterService(new TourRepository(), new ProviderRepository(), new IntegrationService(), (new Mock<ILogger>()).Object);

        [TestMethod]
        public async Task ExecuteAsyncTest()
        {
            bool isOk = false;
            try
            {
                await _importerService.ExecuteAsync(1);
                isOk = true;
            }
            catch (System.Exception)
            {

            }
            Assert.IsTrue(isOk);
        }
    }
}
