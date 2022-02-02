using ApiIntegration.Logic;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace ApiIntegration.UnitTests {
    public class Tests {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void TestDiscountCorrectlyApplied() {
            var logger = new Mock<ILogger<PriceHandler>>();
            var priceHandler = new PriceHandler(logger.Object);
            var result = priceHandler.GetPrice(200, 0.1M, 0.5M);
            Assert.AreEqual(120, result);
        }

        [Test]
        public void DiscountGreaterThan100ThrowsError() {
            var logger = new Mock<ILogger<PriceHandler>>();
            var priceHandler = new PriceHandler(logger.Object);
            TestDelegate getPrice = () => priceHandler.GetPrice(195, 5, 1.01M);
            Assert.Throws<Exception>(getPrice);
        }
    }
}