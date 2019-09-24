namespace ApiIntegration.Tests.PriceAdjuster
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
    using Xunit;
    using PriceAdjuster = ApiIntegration.PriceAdjuster;

    public class PriceAdjusterTests
    {
        [Fact(DisplayName = "Adjust the price on tour availability")]
        public async Task Adjust_The_Price_On_Tour_Availability()
        {
            var providerMock = new Mock<IProviderRepository>();
            providerMock.Setup(p => p.Get(It.IsAny<int>()))
                .ReturnsAsync(new Provider
                {
                    Commission = 0.15m,
                    Name = "Tim's diner'",
                    ProviderId = 1
                });
            
            var priceBuilderMock = new Mock<IPriceBuilder>();
            priceBuilderMock.Setup(b => b.Build(It.IsAny<Provider>(), It.IsAny<decimal>()))
                .Returns(122);
            var adjuster = new PriceAdjuster(providerMock.Object, priceBuilderMock.Object, new Mock<ILogger>().Object);

            var result = await adjuster.Adjust(new List<TourAvailability>
            {
                new TourAvailability
                {
                    AdultPrice = 100,
                    AvailabilityCount = 5,
                    StartDate = new DateTime(2020, 10, 20),
                    TourDuration = 3,
                    TourId = 1
                }
            }, 1);

            result.Count.Should().Be(1);
            
            result.First().Should().BeEquivalentTo(new TourAvailability
            {
                AdultPrice = 122,
                AvailabilityCount = 5, 
                StartDate = new DateTime(2020, 10, 20),
                TourDuration = 3, TourId = 1
            });
        }
    }
}