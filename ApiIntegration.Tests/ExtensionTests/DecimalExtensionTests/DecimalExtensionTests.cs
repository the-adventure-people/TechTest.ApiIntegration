using ApiIntegration.Core.Services.Importer;
using ApiIntegration.Infrastructure.Repositories.Provider;
using ApiIntegration.Infrastructure.Repositories.Tour;
using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ApiIntegration.Core.Extensions;
using FluentAssertions;

namespace ApiIntegration.Tests.ServiceTests.ImporterTests
{
    [TestFixture]
    public class DecimalExtensionTests
    {
        [Test]
        public void AdjustingPrice_YieldsCorrect_Price()
        {
            //Arrange
            decimal price = 500.0m;
            decimal commission = 0.15m;

            //Act
            var adjustedPrice = price.AdjustPrice(commission);

            //Assert
            adjustedPrice.Should().Be(550m);
        }

    }
}
