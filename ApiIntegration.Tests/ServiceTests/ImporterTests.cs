namespace ApiIntegration.Tests.ServiceTests
{
    using ApiIntegration.Interfaces;
    using ApiIntegration.Models;
    using ApiIntegration.Services;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    [TestFixture]
    public class ProviderTests
    {
        private Importer _importer;
        private ITourRepository _tourRepository;
        private IProviderRepository _providerRepository;
        private IApiDownloader _apiDownloader;
        private IPricingService _pricingService;
        private LoggerMock _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new LoggerMock();
            _providerRepository = Mock.Of<IProviderRepository>();
            Mock.Get(_providerRepository).Setup(x => x.Get(It.IsAny<int>())).Returns(Task.FromResult(new Provider { ProviderId = 1 }));

            _tourRepository = Mock.Of<ITourRepository>();
            Mock.Get(_tourRepository).Setup(x => x.Get(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(TestData.TestTour()));

            _apiDownloader = Mock.Of<IApiDownloader>();
            Mock.Get(_apiDownloader).Setup(x => x.Download()).Returns(Task.FromResult(TestData.TestAvailabilityData()));

            _pricingService = Mock.Of<IPricingService>();

            _importer = new Importer(
                _tourRepository,
                _providerRepository,
                _apiDownloader,
                _pricingService,
                _logger
                );
        }


        [Test]
        public async Task Execute_ImportOk()
        {
            // Act
            await _importer.ExecuteAsync(1);

            // Assert
            Mock.Get(_providerRepository).Verify(mock => mock.Get(It.IsAny<int>()), Times.Exactly(1));
            Mock.Get(_tourRepository).Verify(mock => mock.Update(It.IsAny<Tour>()), Times.Exactly(2));
        }

        [Test]
        public async Task Execute_ImportErrorLogged()
        {
            // Arrange
            Mock.Get(_pricingService).Setup(x => x.CalcSellingPrice(It.IsAny<decimal>(), It.IsAny<decimal>())).Throws(new HttpRequestException());

            // Act
            await _importer.ExecuteAsync(1);

            // Assert
            _logger.VerifyWasLogged(LogLevel.Error, "Error occurred during import.");
        }

    }
}