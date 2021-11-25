using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration.Tests.Integration
{
    [TestFixture]
    public class ImporterTests
    {
        [Test]
        public async Task Execute_EndToEnd()
        {
            var mockLogger = Mock.Of<ILogger<ImporterTests>>();

            var tourRepository = new TourRepository();
            var providerRepository = new ProviderRepository();
            var apiDownloader = new ApiDownloader(mockLogger);
            var pricingStrategy = new DiscountedPricingStrategy(mockLogger);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProviderMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var importer = new Importer(tourRepository, providerRepository, apiDownloader, pricingStrategy, mapper, mockLogger);

            await importer.Execute(1);

            var eur123 = await tourRepository.GetAsync("EUR123");
            var eur456 = await tourRepository.GetAsync("EUR456");
            var eur789 = await tourRepository.GetAsync("EUR789");

            Assert.AreEqual(3, eur123.Availabilities.Count);
            Assert.AreEqual(1, eur123.TourId);
            Assert.AreEqual(new DateTime(2020, 6, 20), eur123.Availabilities.First().StartDate);
            Assert.AreEqual(5, eur123.Availabilities.First().TourDuration);
            Assert.AreEqual(8, eur123.Availabilities.First().AvailabilityCount);
            Assert.AreEqual(550, eur123.Availabilities.First().SellingPrice);

            Assert.AreEqual(2, eur456.Availabilities.Count);
            Assert.AreEqual(2, eur456.TourId);
            Assert.AreEqual(new DateTime(2020, 3, 10), eur456.Availabilities.First().StartDate);
            Assert.AreEqual(10, eur456.Availabilities.First().TourDuration);
            Assert.AreEqual(4, eur456.Availabilities.First().AvailabilityCount);
            Assert.AreEqual(880, eur456.Availabilities.First().SellingPrice);

            Assert.IsNull(eur789);
        }
    }
}
