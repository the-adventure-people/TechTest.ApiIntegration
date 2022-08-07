namespace ApiIntegration.Import
{
    using ApiIntegration.Finance;
    using ApiIntegration.Models;
    using ApiIntegration.Providers;
    using ApiIntegration.Repositories;

    using FluentValidation;

    using Microsoft.Extensions.Logging;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class Importer<TProviderResponse> : IImporter<TProviderResponse>
    {
        private readonly IAvailabilityApi<TProviderResponse> _availabilityApi;
        private readonly IProviderResponseAdapter<TProviderResponse> _providerResponseAdapter;
        private readonly IValidator<TProviderResponse?> _providerResponseValidator;

        private readonly IProviderRepository _providerRepository;
        private readonly ITourPricingService _tourPricingService;
        private readonly ITourRepository _tourRepository;
        private readonly ILogger<Importer<TProviderResponse>> _logger;

        public Importer(
            IProviderResponseAdapter<TProviderResponse> providerResponseAdapter,
            IAvailabilityApi<TProviderResponse> availabilityApi,
            IValidator<TProviderResponse?> providerResponseValidator,
            IProviderRepository providerRepository,
            ITourPricingService tourPricingService,
            ITourRepository tourRepository,
            ILogger<Importer<TProviderResponse>> logger)
        {
            _providerResponseAdapter = providerResponseAdapter;
            _availabilityApi = availabilityApi;
            _providerResponseValidator = providerResponseValidator;
            _providerRepository = providerRepository;
            _tourPricingService = tourPricingService;
            _tourRepository = tourRepository;
            _logger = logger;
        }


        public async Task ExecuteAsync(int providerID, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Download Started");

            var provider = await _providerRepository.GetAsync(providerID, cancellationToken);

            if (provider is null)
            {
                _logger.LogError("Unable to find provider from provider ID {providerID}.", providerID); ;
                return;
            }

            var providerResponse = await _availabilityApi.GetAvailabilityDataAsync(cancellationToken);

            var validationResult = await _providerResponseValidator.ValidateAsync(providerResponse, cancellationToken);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Invalid response recieved from API. {validationErrors}", validationResult.GetErrorMessages());
                return;
            }

            var tourAvailabilties = _providerResponseAdapter.ConvertToTourAvailabilityAsync(providerResponse!, cancellationToken);

            var pricedTourAvailabilities = _tourPricingService.PriceTourAvailabilityAsync(provider, tourAvailabilties, cancellationToken);

            await UpdateTourInformationAsync(pricedTourAvailabilities, cancellationToken);

            _logger.LogInformation("Download Finished");
        }

        private async Task UpdateTourInformationAsync(
            IAsyncEnumerable<PricedTourAvailability> pricedTourAvailability,
            CancellationToken cancellationToken = default)
        {
            await foreach (var availabilitiesGroupedByTourID in pricedTourAvailability.GroupBy(x => x.TourId))
            {
                var tourID = availabilitiesGroupedByTourID.Key;
                var tour = await _tourRepository.GetAsync(tourID, cancellationToken);

                if (tour is null)
                {
                    _logger.LogWarning("Attempted to update tour availability for not existing tour ID {tourId}", tourID);
                    continue;
                }

                tour!.Availabilities.Clear();
                tour!.Availabilities.AddRange(await availabilitiesGroupedByTourID.ToListAsync());

                await _tourRepository.UpdateAsync(tour!, cancellationToken);
            }
        }
    }
}
