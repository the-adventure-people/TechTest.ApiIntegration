using ApiIntegration.Interfaces;
using ApiIntegration.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiIntegration.Tests.Services
{
    [TestFixture]
    public class ApiDownloaderTests
    {
        private ApiDownloader apiDownloader;
        private ILogger<ApiDownloader> mockLogger;
        private Mock<IApiDownloaderHttpHandler> mockHttpHandler;

        [SetUp]
        public void Setup()
        {
            mockLogger = Mock.Of<ILogger<ApiDownloader>>();
            mockHttpHandler = new Mock<IApiDownloaderHttpHandler>();
            mockHttpHandler.Setup(m => m.GetBodyAsync()).ReturnsAsync(TestData.AvailabilityApi.ExampleBody);
        }

        [Test]
        public async Task Download_Success()
        {
            // Arrange
            apiDownloader = new ApiDownloader(mockLogger, mockHttpHandler.Object);

            // Act
            var result = await apiDownloader.Download();

            // Assert
            Assert.True(result.StatusCode == 200
                && result.Body.Count == 6);
        }

        [Test]
        public void Download_Fail_InvalidJson()
        {
            // Arrange
            mockHttpHandler.Setup(m => m.GetBodyAsync()).ReturnsAsync(TestData.AvailabilityApi.IncorrectBody);
            apiDownloader = new ApiDownloader(mockLogger, mockHttpHandler.Object);

            // Act and Assert
            Assert.ThrowsAsync<JsonReaderException>(
                async () => await apiDownloader.Download());
        }
    }
}
