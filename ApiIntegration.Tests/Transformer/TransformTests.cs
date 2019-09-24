namespace ApiIntegration.Tests.Transformer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models;
    using Moq;
    using ProviderModels;
    using Xunit;
    using Transformer = ApiIntegration.Transformer;

    public class Transform
    {
        // TODO: test no matching record
        
        [Fact(DisplayName = "Transform One Record")]
        public async Task Transform_One_Record()
        {
            var tourRepo = new Mock<ITourRepository>();
            tourRepo.Setup(t => t.Get(It.IsAny<Func<Tour, bool>>()))
                .ReturnsAsync(new Tour
                {
                    TourId = 1,
                    TourRef = "EUR123",
                    TourName = "Cycling Danube",
                    ProviderId = 1,
                    Active = true,
                    ReviewCount = 13,
                    ReviewScore = 4.3m,
                    Availabilities = new List<TourAvailability>
                    {
                        new TourAvailability
                        {
                            TourId = 1,
                            AdultPrice = 500,
                            StartDate = new DateTime(2020, 6, 20),
                            TourDuration = 6,
                            AvailabilityCount = 9
                        },
                        new TourAvailability
                        {
                            TourId = 1,
                            AdultPrice = 450,
                            StartDate = new DateTime(2020, 6, 27),
                            TourDuration = 6,
                            AvailabilityCount = 9
                        }
                    }
                });

            var transformer = new Transformer(tourRepo.Object, new Mock<ILogger>().Object);
            var input = new ApiAvailabilityResponse
            {
                StatusCode = 200,
                Body = new List<Availability>
                {
                    new Availability
                    {
                        ProductCode = "EUR123",
                        DepartureDate = "2020-06-20",
                        Nights = 5,
                        Price = 450.0m,
                        Spaces = 6
                    }
                }
            };

            //Act
            var result = await transformer.Transform(input, 1);

            result.Count.Should().Be(1);

            var record = result.First();

            record.TourAvailability.Should().BeEquivalentTo(new TourAvailability
            {
                AdultPrice = 450,
                AvailabilityCount = 6,
                StartDate = new DateTime(2020, 6, 20),
                TourDuration = 5,
                TourId = 1
            });
        }
    }
}