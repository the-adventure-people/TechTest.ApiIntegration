using ApiIntegration.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace ApiIntegration.Tests.Integration
{
    [TestFixture]
    public class DiscountedPricingStrategyTests
    {
        [Test]
        public void CalculatePrice_DiscountOnly_DiscountedPriceWithDefaultCommision()
        {
            var discountedPricingStrategy = new DiscountedPricingStrategy(Mock.Of<ILogger>());
            var provider = new Provider
            {
                Name = "Test Provider",
                Discount = 0.05m
            };

            var actualPrice = discountedPricingStrategy.CalculatePrice(provider, 100);

            Assert.AreEqual(95, actualPrice);
        }

        [Test]
        public void CalculatePrice_DiscountNegative_ArgumentException()
        {
            var discountedPricingStrategy = new DiscountedPricingStrategy(Mock.Of<ILogger>());
            var provider = new Provider
            {
                Name = "Test Provider",
                Discount = -0.05m
            };

            Assert.Throws<ArgumentException>(() => discountedPricingStrategy.CalculatePrice(provider, 100));
        }

        [Test]
        public void CalculatePrice_DiscountGreaterThanOne_ArgumentException()
        {
            var discountedPricingStrategy = new DiscountedPricingStrategy(Mock.Of<ILogger>());
            var provider = new Provider
            {
                Name = "Test Provider",
                Discount = 1.05m
            };

            Assert.Throws<ArgumentException>(() => discountedPricingStrategy.CalculatePrice(provider, 100));
        }

        [Test]
        public void CalculatePrice_CommissionNegative_ArgumentException()
        {
            var discountedPricingStrategy = new DiscountedPricingStrategy(Mock.Of<ILogger>());
            var provider = new Provider
            {
                Name = "Test Provider",
                Commission = -0.15m,
                Discount = 0.05m
            };

            Assert.Throws<ArgumentException>(() => discountedPricingStrategy.CalculatePrice(provider, 100));
        }

        [Test]
        public void CalculatePrice_OriginalPriceNegative_ArgumentException()
        {
            var discountedPricingStrategy = new DiscountedPricingStrategy(Mock.Of<ILogger>());
            var provider = new Provider
            {
                Name = "Test Provider",
                Commission = 0.1m,
                Discount = 0.1m
            };

            Assert.Throws<ArgumentException>(() => discountedPricingStrategy.CalculatePrice(provider, -100));
        }

        [Test]
        public void CalculatePrice_ValidCommissionAndDiscount_DiscountedPriceWithCommission()
        {
            var discountedPricingStrategy = new DiscountedPricingStrategy(Mock.Of<ILogger>());
            var provider = new Provider
            {
                Name = "Test Provider",
                Commission = 0.15m,
                Discount = 0.05m
            };

            var actualPrice = discountedPricingStrategy.CalculatePrice(provider, 100);

            Assert.AreEqual(110, actualPrice);
        }
    }
}
