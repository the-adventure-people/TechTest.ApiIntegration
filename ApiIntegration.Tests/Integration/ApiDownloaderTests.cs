using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ApiIntegration.Tests.Unit
{
    [TestFixture]
    public class ApiDownloaderTests
    {
        [Test]
        public async Task DownloadAsync_ValidApiAvailabilityResponse_ValidEndpoint()
        {
            var apiDownloader = new ApiDownloader(Mock.Of<ILogger>());

            var results = await apiDownloader.DownloadAsync<ApiAvailabilityResponse>("http://tap.techtest.s3-website.eu-west-2.amazonaws.com/");

            Assert.IsNotNull(results);
        }

        [Test]
        public async Task DownloadAsync_Null_InvalidEndpoint()
        {
            var apiDownloader = new ApiDownloader(Mock.Of<ILogger>());

            var results = await apiDownloader.DownloadAsync<ApiAvailabilityResponse>("http://tap.techtest1.s3-website.eu-west-2.amazonaws.com/");

            Assert.IsNull(results);
        }

        [Test]
        public void DownloadAsync_InvalidArgumentException_EmptyEndpoint()
        {
            var apiDownloader = new ApiDownloader(Mock.Of<ILogger>());

            Assert.ThrowsAsync<ArgumentException>(async () => await apiDownloader.DownloadAsync<ApiAvailabilityResponse>(""));
        }

        [Test]
        public void DownloadAsync_InvalidOperationException_InvalidUrlEndpoint()
        {
            var apiDownloader = new ApiDownloader(Mock.Of<ILogger>());

            Assert.ThrowsAsync<InvalidOperationException>(async () => await apiDownloader.DownloadAsync<ApiAvailabilityResponse>("someRandomString"));
        }
    }
}
