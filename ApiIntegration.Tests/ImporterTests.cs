using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiIntegration.Tests
{
    class ImporterTests
    {
        private readonly Mock<ITourRepository> mockTourRepository;
        private readonly Mock<IProviderRepository> mockProviderRepository;
        private readonly Mock<IApiDownloader> mockApiDownloader;
        private readonly Mock<IPricingStrategy> mockPricingStrategy;
        private readonly Mock<ILogger<Importer>> mockLogger;
        private Importer importer;

        public Func<Task> Action { get; private set; }

        public ImporterTests()
        {
            mockTourRepository = new Mock<ITourRepository>();
            mockProviderRepository = new Mock<IProviderRepository>();
            mockApiDownloader = new Mock<IApiDownloader>();
            mockPricingStrategy = new Mock<IPricingStrategy>();
            mockLogger = new Mock<ILogger<Importer>>();
        }

        [SetUp]
        public void Setup()
        {
            importer = new Importer(mockTourRepository.Object,
                mockProviderRepository.Object,
                mockApiDownloader.Object,
                mockPricingStrategy.Object,
                mockLogger.Object);
        }

        [Test]
        public void Execute_WithInvalidProviderId_ShouldThrowError()
        {
            var providerId = 1;
            mockProviderRepository.Setup(x => x.Get(providerId)).ReturnsAsync(null as Provider);

            Func<Task> act = async () => await importer.Execute(providerId);
            
            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        public async Task Execute_ShouldUpdateTourAvailabilies()
        {
            // Arrange
            var providerId = 1;
            var refCode = "TestRef";
            var initialPrice = 500;
            var adjustedPrice = 700;

            var provider = new Provider
            {
                ProviderId = providerId,
                Name = "Test Provider",
                Url = "https://testing.com",
                Discount = 0.05m,
                Commission = 0.15m
            };

            var tour = new Tour
            {
                TourId = 1,
                TourRef = refCode,
                Availabilities = new List<TourAvailability>()
                {
                    new TourAvailability
                    {
                        TourId = 1,
                        StartDate = DateTime.UtcNow,
                        TourDuration = 2,
                        AdultPrice = 550,
                        AvailabilityCount = 5
                    }
                }
            };

            var providerAvailability = new Availability
            {
                ProductCode = refCode,
                DepartureDate = DateTime.UtcNow.AddDays(5),
                Nights = 3,
                Price = initialPrice,
                Spaces = 3
            };

            var apiAvailabilityResponse = new ApiAvailabilityResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = new List<Availability> { providerAvailability }
            };

            Tour savedTour = null;
            mockTourRepository.Setup(x => x.Update(It.IsAny<Tour>())).Callback<Tour>(t => savedTour = t);
            mockProviderRepository.Setup(x => x.Get(provider.ProviderId)).ReturnsAsync(provider);
            mockApiDownloader.Setup(x => x.Download(provider.Url)).ReturnsAsync(apiAvailabilityResponse);
            mockTourRepository.Setup(x => x.Get(refCode)).ReturnsAsync(tour);
            mockPricingStrategy.Setup(x => x.AdjustPrice(initialPrice, provider.Discount, provider.Commission))
                .Returns(adjustedPrice);

            // Act
            await importer.Execute(providerId);

            // Assert
            mockTourRepository.Verify(x => x.Update(It.IsAny<Tour>()), Times.Once);
            var savedAvailability = savedTour.Availabilities.Single();

            savedAvailability.AdultPrice.Should().Be(adjustedPrice);
            savedAvailability.AvailabilityCount.Should().Be(providerAvailability.Spaces);
            savedAvailability.StartDate.Should().Be(providerAvailability.DepartureDate);
            savedAvailability.TourDuration.Should().Be(providerAvailability.Nights);
            savedAvailability.TourId.Should().Be(tour.TourId);
        }
    }
}
