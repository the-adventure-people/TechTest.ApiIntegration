using ApiIntegration.Interfaces;
using System;
using System.Collections.Generic;
using ApiIntegration.Services.Tours;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Threading.Tasks;
using ApiIntegration.Models;
using NSubstitute;
using FluentAssertions;

namespace ApiIntegration.UnitTests.TourServiceTests
{
    public class ProviderServiceTests
    {
        private readonly TourService _sut;
        private readonly ITourRepository _tourRepository = Substitute.For<ITourRepository>();
        private readonly ILogger _logger = Substitute.For<ILogger>();

        public ProviderServiceTests()
        {
            _sut = new TourService(_tourRepository, _logger); 
        }

        [Fact]
        public async Task GetAsync_ShouldReturnTour_WhenTourExists()
        {
            // Arrange
            var existingTour = new Tour()
            {
                TourId = 2,
                TourRef = "EUR456",
                TourName = "Cycling Rhine",
                ProviderId = 1,
                Active = true,
                ReviewCount = 55,
                ReviewScore = 4.8m,
                Availabilities = new List<TourAvailability>()
                        {
                            new TourAvailability()
                            {
                                TourId = 2,
                                SellingPrice = 720,
                                StartDate = new DateTime(2020, 3, 10),
                                TourDuration = 11,
                                AvailabilityCount = 4
                            },
                            new TourAvailability()
                            {
                                TourId = 2,
                                SellingPrice = 720,
                                StartDate = new DateTime(2020, 3, 20),
                                TourDuration = 11,
                                AvailabilityCount = 5
                            }
                        }
            };

            _tourRepository.GetAsync(Arg.Any<int>(), Arg.Any<string>()).Returns(existingTour);

            // Act
            var result = await _sut.GetAsync(existingTour.TourId);

            // Assert
            result.Should().BeEquivalentTo(existingTour);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNullTour_WhenTourDoesNotExists()
        {
            // Arrange
            var nullTour = new Tour();
            _tourRepository.GetAsync(Arg.Any<int>(), Arg.Any<string>()).Returns(nullTour);

            // Act
            var result = await _sut.GetAsync(nullTour.TourId);

            // Assert
            result.Should().BeEquivalentTo(nullTour);
        }

        // could test exceptions and _logger calls
    }
}
