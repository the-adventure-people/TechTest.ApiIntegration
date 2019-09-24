using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ApiIntegration.Tests
{

    [TestFixture]
    public class ImporterTests
    {
        private ITourRepository tourRepository;
        private IProviderRepository providerRepository;
        private IApiDownloader apiDownloader;
        private ILogger logger;

        [SetUp]
        public void Init()
        {
            tourRepository = new TourRepository();
            providerRepository = new ProviderRepository();
            logger = new DummyLogger();
        }

        [TearDown]
        public void Dispose()
        {
            tourRepository = null;
            providerRepository = null;
            apiDownloader = null;
            logger = null;
        }


        [Test]
        public async Task FullyImported_ValidProvider_ValidUrl_ShouldSucceedAsync()
        {
            apiDownloader = new ApiDownloader("http://tap.techtest.s3-website.eu-west-2.amazonaws.com/");
            int originalEUR123Count = tourRepository.Get(-1, "EUR123").Result.Availabilities.Count;
            int originalEUR456Count = tourRepository.Get(-1, "EUR456").Result.Availabilities.Count;

            IImporter importer  = new Importer( tourRepository, providerRepository, apiDownloader, logger );
            await importer.Execute(1);

            int updatedEUR123Count = tourRepository.Get(-1, "EUR123").Result.Availabilities.Count;
            Assert.AreEqual(originalEUR123Count + 3, updatedEUR123Count, "Importer failed to import the three EUR123 records from URL");

            int updatedEUR456Count = tourRepository.Get(-1, "EUR456").Result.Availabilities.Count;
            Assert.AreEqual(originalEUR123Count + 2, updatedEUR456Count, "Importer failed to import the two EUR456 records from URL");

            var shouldBeNull = await tourRepository.Get(-1, "EUR789");
            Assert.IsNull(shouldBeNull, "Importer shouldn't import EUR789 records from URL");
        }



        [Test]
        public async Task FullyImported_InvalidProvider_ValidUrl_ShouldNotImportAsync()
        {
            apiDownloader = new ApiDownloader("http://tap.techtest.s3-website.eu-west-2.amazonaws.com/");

            IImporter importer = new Importer(tourRepository, providerRepository, apiDownloader, logger);
            try
            {
                await importer.Execute(-1);
                Assert.Fail($"Importer failed to throw any exception");
            }
            catch (Exception ex)
            {
                Assert.Pass($"Importer throw valid exception {ex}");
            }

        }


        [Test]
        public async Task FullyImported_InvalidUrl_ShouldSucceedAsync()
        {
            apiDownloader = new ApiDownloader("qwerty");

            IImporter importer = new Importer(tourRepository, providerRepository, apiDownloader, logger);

            try
            {
                await importer.Execute(1);
                Assert.Fail($"Importer failed to throw any exception");
            }
            catch (Exception ex)
            {
                Assert.Pass($"Importer throw valid exception {ex}");
            }
        }

    }
}
