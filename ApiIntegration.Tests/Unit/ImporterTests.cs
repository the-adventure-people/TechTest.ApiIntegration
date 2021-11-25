using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace ApiIntegration.Tests.Unit
{
    [TestFixture]
    public class ImporterTests
    {
        private Mock<ILogger<Importer>> _loggerMock;
        private Mock<ITourRepository> _tourRepositoryMock;
        private Mock<IProviderRepository> _providerRepositoryMock;
        private Mock<IApiDownloader> _apiDownloaderMock;
        private Mock<IPricingStrategy> _pricingStrategyMock;
        private IMapper _mapper;
        private Importer _importerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<Importer>>();
            _tourRepositoryMock = new Mock<ITourRepository>();
            _providerRepositoryMock = new Mock<IProviderRepository>();
            _apiDownloaderMock = new Mock<IApiDownloader>();
            _pricingStrategyMock = new Mock<IPricingStrategy>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProviderMappingProfile>();
            });
            _mapper = new Mapper(configuration);

            _importerMock = new Importer(
                _tourRepositoryMock.Object,
                _providerRepositoryMock.Object,
                _apiDownloaderMock.Object,
                _pricingStrategyMock.Object,
                _mapper,
                _loggerMock.Object);
        }

        [Test]
        public void Execute_InvalidProvider_ArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _importerMock.Execute(2));
        }

        // Ideally I'd like to work towards mocking an end-to-end unit test
    }
}
