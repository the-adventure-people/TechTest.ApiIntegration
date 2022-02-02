namespace ApiIntegration.Tests.ServiceTests
{
    using ApiIntegration.Services;
    using ApiIntegration.Tests.Mocks;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    [TestFixture]
    public class ApiDownloaderTests
    {
        private ApiDownloader _apiDownloader;

        [Test]
        public async Task Download_Ok()
        {
            // Arrange
            var mockHttpClientFactory = new HttpClientFactoryMock();
            _apiDownloader = new ApiDownloader(mockHttpClientFactory);

            // Act
            var result = await _apiDownloader.Download();

            // Assert
            Assert.That(result.Body.Count, Is.EqualTo(2));
        }

        [Test]
        public void Download_Error()
        {
            // Arrange
            var mockHttpClientFactory = new HttpClientFactoryMock("http://fake.endpoint400");
            _apiDownloader = new ApiDownloader(mockHttpClientFactory);

            var ex = Assert.ThrowsAsync<HttpRequestException>(async () => await _apiDownloader.Download());
        }
    }
}