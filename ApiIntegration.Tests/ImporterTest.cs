using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ApiIntegration.Tests
{
    [TestFixture]
    class ImporterTest
    {
        private readonly ApiAvailabilityResponse _availabilityResponse = new ApiAvailabilityResponse()
        {
            Body = new List<Availability>()
            {
                new Availability()
                {
                    ProductCode = "EUR123",
                    DepartureDate = "2020-06-20",
                    Nights =  5,
                    Price = 500,
                    Spaces = 8
                },
                new Availability()
                {
                    ProductCode = "EUR123",
                    DepartureDate = "2020-06-27",
                    Nights =  5,
                    Price = 450,
                    Spaces = 4
                },
                new Availability()
                {
                    ProductCode = "EUR123",
                    DepartureDate = "2020-07-04",
                    Nights =  5,
                    Price = 500,
                    Spaces = 6
                },
                new Availability()
                {
                    ProductCode = "EUR456",
                    DepartureDate = "2020-03-10",
                    Nights =  10,
                    Price = 800,
                    Spaces = 4
                },
                new Availability()
                {
                    ProductCode = "EUR456",
                    DepartureDate = "2020-03-20",
                    Nights =  10,
                    Price = 800,
                    Spaces = 5
                },
                new Availability()
                {
                    ProductCode = "EUR789",
                    DepartureDate = "2020-09-20",
                    Nights =  4,
                    Price = 250,
                    Spaces = 9
                }
            },
            StatusCode = 200
        };

        private readonly List<TourAvailability> _tourAvailabilities = new List<TourAvailability>()
        {
            new TourAvailability()
            {
                TourId = 1,
                StartDate = new DateTime(2020, 6, 20),
                TourDuration = 5,
                AdultPrice = 475.15m,
                AvailabilityCount = 8
            },
            new TourAvailability()
            {
                TourId = 1,
                StartDate = new DateTime(2020, 6, 27),
                TourDuration = 5,
                AdultPrice = 427.65m,
                AvailabilityCount = 4
            },
            new TourAvailability()
            {
                TourId = 1,
                StartDate = new DateTime(2020, 7, 4),
                TourDuration = 5,
                AdultPrice = 475.15m,
                AvailabilityCount = 6
            },
            new TourAvailability()
            {
                TourId = 2,
                StartDate = new DateTime(2020, 3, 10),
                TourDuration = 10,
                AdultPrice = 760.15m,
                AvailabilityCount = 4
            },
            new TourAvailability()
            {
                TourId = 2,
                StartDate = new DateTime(2020, 3, 20),
                TourDuration = 10,
                AdultPrice = 760.15m,
                AvailabilityCount = 5
            }
        };

        [Test]
        public void Execute()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var tourRepository = new TourRepository();
                var providerRepository = new ProviderRepository();

                var apiDownloader = new Mock<IApiDownloader>();
                apiDownloader.Setup(p => p.Download()).ReturnsAsync(_availabilityResponse);
                var logger = Mock.Of<ILogger>();

                var importer = new Importer(tourRepository, providerRepository, apiDownloader.Object, logger);
                await importer.Execute(1);

                for (int index = 1; index < 3; index++)
                {
                    var tour = await tourRepository.Get(index);
                    var availabilities = _tourAvailabilities.Where(p => p.TourId == tour.TourId).ToArray();
                    Assert.AreEqual(availabilities.Length, tour.Availabilities.Count);

                    for (var i = 0; i < availabilities.Length; i++)
                    {
                        var availability = availabilities[i];
                        var tourAvailability = tour.Availabilities[i];

                        Assert.AreEqual(availability.AdultPrice, tourAvailability.AdultPrice);
                        Assert.AreEqual(availability.AvailabilityCount, tourAvailability.AvailabilityCount);
                        Assert.AreEqual(availability.StartDate, tourAvailability.StartDate);
                        Assert.AreEqual(availability.TourDuration, tourAvailability.TourDuration);
                        Assert.AreEqual(availability.TourId, tourAvailability.TourId);
                    }
                }
            });
        }

        [Test]
        public void DoesNotThrowOnEmptyTourList()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var tourRepository = new Mock<ITourRepository>();
                tourRepository.Setup(p => p.Get(1)).ReturnsAsync(await Task.FromResult<Tour>(null));
                var providerRepository = new ProviderRepository();

                var apiDownloader = new Mock<IApiDownloader>();
                apiDownloader.Setup(p => p.Download()).ReturnsAsync(_availabilityResponse);
                var logger = Mock.Of<ILogger>();

                var importer = new Importer(tourRepository.Object, providerRepository, apiDownloader.Object, logger);
                await importer.Execute(1);
            });
        }
    }
}
