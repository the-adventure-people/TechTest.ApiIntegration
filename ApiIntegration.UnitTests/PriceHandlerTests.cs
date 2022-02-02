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
            var logger = new Mock<ILogger<SalePriceHandler>>();
            var priceHandler = new SalePriceHandler(logger.Object);
            var result = priceHandler.GetPrice(200, 0.1M);
            Assert.AreEqual(210, result);
        }
    }
}