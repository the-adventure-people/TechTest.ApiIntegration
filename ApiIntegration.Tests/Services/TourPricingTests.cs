using ApiIntegration.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Tests.Services
{
    [TestFixture]
    public class TourPricingTests
    {
        private TourPricing tourPricing;
        private Mock<IConfiguration> mockConfig;

        [SetUp]
        public void Setup()
        {
            mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["PricingDiscount"]).Returns("5");
        }


        [Test]
        public void CalculateWebsitePrice_Correct()
        {
            // Arrange
            tourPricing = new TourPricing(mockConfig.Object);
            var providerPrice = 500;
            decimal comission = 0.15m;


            // Act
            var result = tourPricing.CalculateWebsitePrice(new Models.CalculateWebsitePriceRequest
            {
                ProviderPrice = providerPrice,
                ComissionPercentage = comission
            });

            // Assert
            Assert.IsTrue(result == 550);
        }
    }
}
