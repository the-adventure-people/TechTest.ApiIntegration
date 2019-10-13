using System.Threading.Tasks;
using ApiIntegration.Interfaces;
using NUnit.Framework;

namespace ApiIntegration.Tests
{
    [TestFixture]
    class ApiDownloaderTest
    {
        [Test]
        public async Task Download()
        {
            IApiDownloader apiDownloader = new ApiDownloader();
            var response = await apiDownloader.Download();

            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(6, response.Body.Count);

            var availability = response.Body[3];

            Assert.AreEqual("2020-03-10", availability.DepartureDate);
            Assert.AreEqual(10, availability.Nights);
            Assert.AreEqual(800, availability.Price);
            Assert.AreEqual("EUR456", availability.ProductCode);
            Assert.AreEqual(4, availability.Spaces);
        }
    }
}
