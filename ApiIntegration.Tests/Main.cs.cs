
using ApiIntegration.Data.Models.External;
using ApiIntegration.Data.Repositories;
using ApiIntegration.Services;
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
            var tourService = new TourService(tourRepo, Substitute.For<ILogger<TourService>>());
            var provService = new ProviderService(providerRepo, Substitute.For<ILogger<ProviderService>>());
            var financeservice = new FinanceService(providerRepo);
            var apiDownloaderService = new AvailabilityApiDownloaderService(Substitute.For<ILogger<IApiDownloaderService<ApiAvailabilityResponse>>>(), new HttpClient(), "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/");

            var sut = new ImporterService(tourService, apiDownloaderService, financeservice, Substitute.For<ILogger<IImporterService>>());

            // Act
            await sut.ExecuteAsync(1);

            // Assert
            Assert.True(1 == 1);
        }
    }
}
