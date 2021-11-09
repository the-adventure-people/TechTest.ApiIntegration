using ApiIntegration.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ApiIntegration.Tests
{

    [TestFixture]
    public class ImporterTests : IDisposable
    {
        public IImporter Client;
        private Mock<ITourRepository> TourRepo;
        private Mock<IProviderRepository> ProviderRepo;
        private Mock<IApiDownloader> ApiDownloader;
        private Mock<ILogger> logger;

        [SetUp]
        public void Setup()
        {
            this.TourRepo = new Mock<ITourRepository>(MockBehavior.Strict);
            this.ProviderRepo = new Mock<IProviderRepository>(MockBehavior.Strict);
            this.ApiDownloader = new Mock<IApiDownloader>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.Client = new Importer(this.TourRepo.Object, this.ProviderRepo.Object, this.ApiDownloader.Object, this.logger.Object);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task ExecuteTest(int id)
        {
            // arrange
            this.logger.Setup(e => e.LogInformation(It.IsAny<string>()));
            this.ApiDownloader.Setup(e => e.Download()).ReturnsAsync(new ProviderModels.ApiAvailabilityResponse());

            /*my logic*/
            this.logger.Setup(e => e.LogInformation(It.IsAny<string>()));
            // act
            await this.Client.Execute(id);
            // assert
            this.logger.Verify(e => e.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.ApiDownloader.Verify(e => e.Download(), Times.Exactly(1));
            Assert.That(true);
        }

        [TearDown]
        public void Dispose()
        {
            this.Client = null;
            this.ApiDownloader = null;
            this.logger = null;
            this.ProviderRepo = null;
            this.TourRepo = null;
            GC.SuppressFinalize(true);
        }
    }
}
