namespace ApiIntegration.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using ApiIntegration.Finance;
    using ApiIntegration.Import;
    using ApiIntegration.Models;
    using ApiIntegration.Providers;
    using ApiIntegration.Providers.AwesomeCyclingHolidays.Models;
    using ApiIntegration.Repositories;

    using FluentValidation;
    using FluentValidation.Results;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    using Moq;

    using NUnit.Framework;

    public class ImporterTests
    {
        [Test]
        public async Task ExecuteAsync_ExitsEarly_WhenNoProviderFound()
        {
            // arrange
            var availabilityApi = GetLooseMockService<IAvailabilityApi<AvailabilityResponse>>();
            var providerResponseAdapter = GetLooseMockService<IProviderResponseAdapter<AvailabilityResponse>>();
            var providerResponseValidator = GetMockValidator();

            var providerRepository = GetMockProviderRepository();
            var tourPricingService = GetMockPricingService();
            var tourRepository = GetLooseMockService<ITourRepository>();
            var logger = new NullLogger<Importer<AvailabilityResponse>>();

            var importer = new Importer<AvailabilityResponse>(
                providerResponseAdapter.Object,
                availabilityApi.Object,
                providerResponseValidator.Object,
                providerRepository.Object,
                tourPricingService.Object,
                tourRepository.Object,
                logger);

            // act
            await importer.ExecuteAsync(1);

            // assert
            providerRepository.Verify(
                x => x.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Once);

            availabilityApi.Verify(
                x => x.GetAvailabilityDataAsync(It.IsAny<CancellationToken>()),
                Times.Never);

            providerResponseValidator.Verify(
                x => x.ValidateAsync(It.IsAny<AvailabilityResponse>(), It.IsAny<CancellationToken>()),
                Times.Never);

            providerResponseAdapter.Verify(
                x => x.ConvertToTourAvailabilityAsync(It.IsAny<AvailabilityResponse>(), It.IsAny<CancellationToken>()),
                Times.Never);

            tourPricingService.Verify(
                x => x.PriceTourAvailabilityAsync(It.IsAny<Provider>(), It.IsAny<IAsyncEnumerable<TourAvailability>>(), It.IsAny<CancellationToken>()),
                Times.Never);

            tourRepository.Verify(
                x => x.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Test]
        public async Task ExecuteAsync_ExitsEarly_WhenProviderResponseIsntValid()
        {
            // arrange
            var availabilityApi = GetLooseMockService<IAvailabilityApi<AvailabilityResponse>>();
            var providerResponseAdapter = GetLooseMockService<IProviderResponseAdapter<AvailabilityResponse>>();
            var providerResponseValidator = GetMockValidator(new ValidationResult(new[] { new ValidationFailure("Code", "FAILED CODE.") }));

            var providerRepository = GetMockProviderRepository(new Provider { ProviderId = 1, Name = "TEST" });
            var tourPricingService = GetMockPricingService();
            var tourRepository = GetLooseMockService<ITourRepository>();
            var logger = new NullLogger<Importer<AvailabilityResponse>>();

            var importer = new Importer<AvailabilityResponse>(
                providerResponseAdapter.Object,
                availabilityApi.Object,
                providerResponseValidator.Object,
                providerRepository.Object,
                tourPricingService.Object,
                tourRepository.Object,
                logger);

            // act
            await importer.ExecuteAsync(1);

            // assert
            providerRepository.Verify(
                x => x.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Once);

            availabilityApi.Verify(
                x => x.GetAvailabilityDataAsync(It.IsAny<CancellationToken>()),
                Times.Once);

            providerResponseValidator.Verify(
                x => x.ValidateAsync(It.IsAny<AvailabilityResponse>(), It.IsAny<CancellationToken>()),
                Times.Once);

            providerResponseAdapter.Verify(
                x => x.ConvertToTourAvailabilityAsync(It.IsAny<AvailabilityResponse>(), It.IsAny<CancellationToken>()),
                Times.Never);

            tourPricingService.Verify(
                x => x.PriceTourAvailabilityAsync(It.IsAny<Provider>(), It.IsAny<IAsyncEnumerable<TourAvailability>>(), It.IsAny<CancellationToken>()),
                Times.Never);

            tourRepository.Verify(
                x => x.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        public Mock<ITourPricingService> GetMockPricingService(
            IEnumerable<PricedTourAvailability>? pricedTourAvailabilities = null)
        {
            pricedTourAvailabilities ??= Array.Empty<PricedTourAvailability>();

            var mock = new Mock<ITourPricingService>();

            mock.Setup(m => m.PriceTourAvailabilityAsync(
                    It.IsAny<Provider>(),
                    It.IsAny<IAsyncEnumerable<TourAvailability>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(pricedTourAvailabilities.ToAsyncEnumerable());

            return mock;
        }

        public Mock<IProviderRepository> GetMockProviderRepository(
            Provider? provider = null)
        {
            var mock = new Mock<IProviderRepository>();

            mock.Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(provider);

            return mock;
        }

        public Mock<IValidator<AvailabilityResponse?>> GetMockValidator(
            ValidationResult? result = null)
        {
            var mock = new Mock<IValidator<AvailabilityResponse?>>();

            mock.Setup(m => m.ValidateAsync(It.IsAny<AvailabilityResponse>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            return mock;
        }

        public Mock<TInterface> GetLooseMockService<TInterface>()
            where TInterface : class
        {
            var mock = new Mock<TInterface>(MockBehavior.Loose);

            return mock;
        }
    }

}
