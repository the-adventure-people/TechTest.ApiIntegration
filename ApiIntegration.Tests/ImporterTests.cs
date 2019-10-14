using System;
using System.Collections.Generic;
using System.Linq;
using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;

namespace ApiIntegration.Tests
{
    [TestFixture]
    public class ImporterTests
    {
        private readonly ApiAvailabilityResponse _emptyAvailabilityResponse = new ApiAvailabilityResponse
        {
            StatusCode = 200,
            Body = new List<Availability>()
        };

        private readonly ApiAvailabilityResponse _newUpdatesForTour1 = new ApiAvailabilityResponse
        {
            StatusCode = 200,
            Body = new List<Availability>
            {                
                new Availability
                {
                    ProductCode = "EUR123",
                    DepartureDate = "2020-06-20",
                    Nights =  5,
                    Price = 500.0m,
                    Spaces = 8
                },                
                new Availability
                {
                    ProductCode = "EUR123",
                    DepartureDate = "2020-06-27",
                    Nights =  5,
                    Price = 450.0m,
                    Spaces = 4
                },
                new Availability
                {
                    ProductCode = "EUR123",
                    DepartureDate = "2020-07-04",
                    Nights =  5,
                    Price = 500.0m,
                    Spaces = 6
                }
            }
        };

        [Test]
        public void Updates_to_existing_tours_succeed()
        {
            //TODO - check values are updated
            Assert.DoesNotThrowAsync(async () =>
            {
                var tourRepository = new TourRepository();
                var providerRepository = new ProviderRepository(); 
                var logger = new Mock<ILogger>();
                var apiDownloader = new Mock<IApiDownloader>();

                apiDownloader.Setup(api => api.Download()).ReturnsAsync(_newUpdatesForTour1);

                var importer = new Importer(
                    tourRepository: tourRepository,
                    providerRepository: providerRepository,
                    apiDownloader: apiDownloader.Object, 
                    logger: logger.Object);

                await importer.Execute(providerId: 1);

                var tour1 = tourRepository.Get(tourId: 1);
                tour1.Result.Availabilities.Count.Should().Be(_newUpdatesForTour1.Body.Count);

                var orderedResults = tour1.Result.Availabilities.OrderBy(a => a.StartDate).ToList();

                for (var i = 0; i < _newUpdatesForTour1.Body.Count; i++)
                {
                    orderedResults[i].StartDate.Should().Be(DateTime.Parse(_newUpdatesForTour1.Body[i].DepartureDate));
                    //TODO - test the pricing algorithm explicitly
                    orderedResults[i].AdultPrice.Should().Be(_newUpdatesForTour1.Body[i].Price + (_newUpdatesForTour1.Body[i].Price * 0.15m) - (_newUpdatesForTour1.Body[i].Price * 0.95m));
                    orderedResults[i].TourDuration.Should().Be(_newUpdatesForTour1.Body[i].Nights);
                    orderedResults[i].AvailabilityCount.Should().Be(_newUpdatesForTour1.Body[i].Spaces);
                }
            });
        }

        [Test]
        public void Can_handle_no_updates()
        {
            //TODO - also check the actual objects themselves are the same
            Assert.DoesNotThrowAsync(async () =>
            {
                var tourRepository = new TourRepository();
                var providerRepository = new ProviderRepository();
                var logger = new Mock<ILogger>();
                var apiDownloader = new Mock<IApiDownloader>();

                apiDownloader.Setup(api => api.Download()).ReturnsAsync(_emptyAvailabilityResponse);

                var importer = new Importer(
                    tourRepository: tourRepository,
                    providerRepository: providerRepository,
                    apiDownloader: apiDownloader.Object,
                    logger: logger.Object);

                await importer.Execute(1);
            });
        }
    }
}
