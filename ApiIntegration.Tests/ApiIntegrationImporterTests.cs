using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Tests
{
    [TestFixture]
    public class ApiIntegrationImporterTests
    {
        private Importer _importer;
        private TourRepository _tourRepository;
        private ProviderRepository _providerRepository;
        private ApiDownloader _apiDownloader;
        private ILogger<ConsoleLoggerProvider> _logger;

        

        [SetUp]
        public void SetUp()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            _logger = loggerFactory.CreateLogger<ConsoleLoggerProvider>();
            _tourRepository = new TourRepository();
            _providerRepository = new ProviderRepository();
            _apiDownloader = new ApiDownloader();           
            _importer = new Importer(_tourRepository, _providerRepository, _apiDownloader, _logger);
        }

        [Test]
        public async Task CanDownload()
        {
            await _importer.Execute(1);
        }
    }
}
