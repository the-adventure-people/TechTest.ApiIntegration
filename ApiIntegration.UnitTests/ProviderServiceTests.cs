using ApiIntegration.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using ApiIntegration.Services.Providers;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Threading.Tasks;
using ApiIntegration.Models;
using System.Linq;
using NSubstitute;
using FluentAssertions;

namespace ApiIntegration.UnitTests.ProviderServiceTests
{
    public class ProviderServiceTests
    {
        private readonly ProviderService _sut;
        private readonly IProviderRepository _ProviderRepository = Substitute.For<IProviderRepository>();
        private readonly ILogger _logger = Substitute.For<ILogger>();

        public ProviderServiceTests()
        {
            _sut = new ProviderService(_ProviderRepository, _logger); 
        }

        [Fact]
        public async Task GetAsync_ShouldReturnProvider_WhenProviderExists()
        {
            // Arrange
            var existingProvider = new Provider()
            {
                ProviderId = 1,
                Name = "Awesome Cycling Holidays",
                Commission = 0.15m
            };

            _ProviderRepository.GetAsync(Arg.Any<int>()).Returns(existingProvider);

            // Act
            var result = await _sut.Get(existingProvider.ProviderId);

            // Assert
            result.Should().BeEquivalentTo(existingProvider);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNullProvider_WhenProviderDoesNotExists()
        {
            // Arrange
            var nullProvider = new Provider();
            _ProviderRepository.GetAsync(Arg.Any<int>()).Returns(nullProvider);

            // Act
            var result = await _sut.Get(nullProvider.ProviderId);

            // Assert
            result.Should().BeEquivalentTo(nullProvider);
        }
    }
}
