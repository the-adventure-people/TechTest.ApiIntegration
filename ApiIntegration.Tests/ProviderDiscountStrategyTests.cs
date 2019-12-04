using NUnit.Framework;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using System;

namespace ApiIntegration.Tests
{
    class ProviderDiscountStrategyTests
    {
        private readonly Mock<ILogger> mockedLogger;
        private ProviderDiscountStrategy discountStrategy;

        public ProviderDiscountStrategyTests()
        {
            mockedLogger = new Mock<ILogger>();
        }

        [SetUp]
        public void Setup()
        {
            discountStrategy = new ProviderDiscountStrategy(mockedLogger.Object);
        }

        [Test]
        public void AdjustPrice_DiscountOnly()
        {
            var initialPrice = 1.0m;
            var discount = 0.5m;
            var commission = 0.0m;

            var result = discountStrategy.AdjustPrice(initialPrice, discount, commission);

            result.Should().Be(0.5m);
        }

        [Test]
        public void AdjustPrice_CommissionOnly()
        {
            var initialPrice = 1.0m;
            var discount = 0.0m;
            var commission = 0.25m;

            var result = discountStrategy.AdjustPrice(initialPrice, discount, commission);

            result.Should().Be(1.25m);
        }

        [Test]
        public void AdjustPrice_DiscountAndCommission()
        {
            var initialPrice = 1.0m;
            var discount = 0.05m;
            var commission = 0.15m;

            var result = discountStrategy.AdjustPrice(initialPrice, discount, commission);

            result.Should().Be(1.10m);
        }

        [Test]
        public void AdjustPrice_ShouldThrowError_ForNegativeCommission()
        {
            var initialPrice = 1.0m;
            var discount = 0.0m;
            var commission = -0.15m;

            Action act = () => discountStrategy.AdjustPrice(initialPrice, discount, commission);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void AdjustPrice_ShouldThrowError_ForNegativeDiscount()
        {
            var initialPrice = 1.0m;
            var discount = -0.1m;
            var commission = 0.0m;

            Action act = () => discountStrategy.AdjustPrice(initialPrice, discount, commission);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void AdjustPrice_ShouldThrowError_ForDiscountGreaterThanOneHundredPercent()
        {
            var initialPrice = 1.0m;
            var discount = 1.1m;
            var commission = 0.0m;

            Action act = () => discountStrategy.AdjustPrice(initialPrice, discount, commission);

            act.Should().Throw<ArgumentException>();
        }
    }
}
