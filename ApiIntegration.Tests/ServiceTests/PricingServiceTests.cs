namespace ApiIntegration.Tests.ServiceTests
{
    using ApiIntegration.Interfaces;
    using ApiIntegration.Services;
    using NUnit.Framework;

    [TestFixture]
    public class PricingServiceTests
    {
        private IPricingService _pricingService;


        [SetUp]
        public void SetUp()
        {
            _pricingService = new PricingService();
        }


        [TestCase("120.0", "0.10", "126.0")]
        [TestCase("400.0", "0.20", "460.0")]
        public void CalcSellingPrice_WithDiscount(decimal price, decimal commissionPercentage, decimal expectedSellingPrice)
        {
            //Arrange
            _pricingService = new PricingService();


            // Act
            var sellingPrice = _pricingService.CalcSellingPrice(price, commissionPercentage);

            // Assert
            Assert.That(sellingPrice, Is.EqualTo(expectedSellingPrice));
        }

        [TestCase("120.0", "0.10")]
        public void CalcSellingPrice_NoDiscount(decimal price, decimal commissionPercentage)
        {
            //Arrange
            _pricingService = new PricingService(0.0m);

            // Act
            var sellingPrice = _pricingService.CalcSellingPrice(price, commissionPercentage);

            // Assert
            Assert.That(sellingPrice, Is.EqualTo(132.0m));
        }

    }
}