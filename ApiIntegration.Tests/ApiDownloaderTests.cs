using System.Net;
using System.Threading.Tasks;
using ApiIntegration.Interfaces;
using FluentAssertions;
using NUnit.Framework;

namespace ApiIntegration.Tests
{
    [TestFixture]
    public class ApiDownloaderTests
    {
        private IApiDownloader _apiDownloader;

        [Test]
        public async Task Api_request_succeeds()
        {
            _apiDownloader = new ApiDownloader();
            var apiResponse = await _apiDownloader.Download();

            apiResponse.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
