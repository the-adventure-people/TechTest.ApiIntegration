using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using ApiIntegration.Services.ApiDownloader;
using ApiIntegration.Services.Providers;
using ApiIntegration.Services.Tours;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiIntegration.Tests
{
    public class ImporterServiceTests
    {
        // Used to Run the Execute
        [Fact]
        public async Task Runner()
        {
            // Arrange
            var tourRepo = new TourRepository();
            var providerRepo = new ProviderRepository();
            var tourService = new TourService(tourRepo, Substitute.For<ILogger>());
            var provService = new ProviderService(providerRepo, Substitute.For<ILogger>());
            var apiDownloaderService = new ApiDownloaderService();

            var sut = new ImporterService(tourService, provService, apiDownloaderService, Substitute.For<ILogger>());

            // Act
            await sut.Execute(1);

            // Assert
            Assert.True(1 == 1);
        }
    }
}
