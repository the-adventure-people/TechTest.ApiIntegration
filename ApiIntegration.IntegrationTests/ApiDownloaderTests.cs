using ApiIntegration.Logic;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiIntegration.IntegrationTests {
    public class Tests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public async Task TestFileDownloadsSuccessfully() {
            using (var httpClient = new HttpClient()){
                var apiDownloaderClient = new ApiDownloaderClient(httpClient, "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/");
                var loggerMock = new Mock<ILogger<ApiDownloader>>();
                var apiDownloader = new ApiDownloader(apiDownloaderClient, loggerMock.Object);
                var response = await apiDownloader.Download();
                Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
                Assert.IsNotEmpty(response.Body);
            }
        }
    }
}