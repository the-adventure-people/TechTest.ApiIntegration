using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ApiIntegration.Tests
{
    [TestFixture]
    class tests
    {
        [Test]
        public async Task TestImporter()
        {
            ILoggerFactory FakeLoggerFactory = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory();
            var fakelogger = FakeLoggerFactory.CreateLogger("Fake");
            var apidownload = new ApiDownloader();
            var TourRepo = new TourRepository();
            var ProvidRepo = new ProviderRepository();

            var importer = new Importer(TourRepo, ProvidRepo, apidownload, fakelogger);
            await importer.Execute(1, 0.05m);
        }
    }
}
