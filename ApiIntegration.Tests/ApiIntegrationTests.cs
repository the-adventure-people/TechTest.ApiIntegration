using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Tests
{
    [TestFixture]
    public class ApiIntegrationTests
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
        public async Task ImporterSuccess()
        {
            await _importer.Execute(1);

            //no need to assert value as this will fail if the Execute fails.
        }

        [Test]
        public void DownloadSuccess()
        {
            var data = _apiDownloader.Download().Result;
            Assert.NotNull(data);
            Assert.True(data.Body.Any());
            Assert.AreEqual(data.StatusCode, 200);
        }

        [Test]
        public void ConversionSuccess()
        {
            var data = _apiDownloader.Download().Result;
            ModelConverter converter = new ModelConverter();
            var convert = converter.ConvertToTourAvailability(data);

            //check successfully converted value to a datetime
            Assert.True(convert.First().StartDate.GetType() == typeof(DateTime), "not a datetime");
            //ints cant be null so will like return zero if they failed to convert
            Assert.NotZero(convert.First().TourId);
            Assert.True(convert.First().TourId.GetType() == typeof(Int32), "not an int32");
        }
    }
}
