using ApiIntegration.Interfaces;
using ApiIntegration.Services.ApiDownloader;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ApiIntegration.ProviderModels;
using ApiIntegration.Models;
using System.Collections.Generic;
using ApiIntegration.Services.Tours;
using ApiIntegration.Services.Providers;

namespace ApiIntegration
{
    public class ImporterService : IImporterService
    {
        private readonly ITourService _tourService;
        private readonly IProviderService _providerRepository;
        private readonly IApiDownloaderService _apiDownloaderService;
        private readonly ILogger _logger;

        public ImporterService(
            ITourService tourRepository,
            IProviderService providerRepository,
            IApiDownloaderService apiDownloader, 
            ILogger logger)
        {
            _tourService = tourRepository;
            _providerRepository = providerRepository;
            _apiDownloaderService = apiDownloader;
            _logger = logger;
        }


        public async Task Execute(int providerId)
        {
            _logger.LogInformation("Download Started");

            var providerResponse = await _apiDownloaderService.Download();
            var tourAvailabilitesFromProvider = await MapTourAvailabilitiesFromProvider(providerResponse, providerId);
            await SaveTourToRepository(tourAvailabilitesFromProvider, providerId);

            _logger.LogInformation("Download Finished");
        }

        private async Task SaveTourToRepository(List<TourAvailability> tourAvailabilites, int providerId)
        {
            try
            {
                foreach (var tourAvailability in tourAvailabilites)
                {
                    var tour = await _tourService.GetAsync(providerId);

                    if (!(tour is null))
                    {
                        tour.Availabilities.Add(tourAvailability);
                        await _tourService.UpdateAsync(tour);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Save To Store Failed due to exception {ex.Message}");
            }
        }

        private async Task<List<TourAvailability>> MapTourAvailabilitiesFromProvider(ApiAvailabilityResponse providerResponse, int providerId)
        {
            var availabilityResults = providerResponse.Body;
            var tourAvailabilities = new List<TourAvailability>();

            foreach (var availabilityResult in availabilityResults)
            {
                var tour = await _tourService.GetAsync(providerId, availabilityResult.ProductCode);

                tourAvailabilities.Add(new TourAvailability
                {
                    TourId = tour.TourId,
                    TourDuration = availabilityResult.Nights,
                    StartDate = availabilityResult.DepartureDate,
                    AvailabilityCount = availabilityResult.Spaces,
                    SellingPrice = await AdjustToSellingPrice(availabilityResult.Price, providerId)
                });
            }

            return tourAvailabilities;
        }

        private async Task<decimal> AdjustToSellingPrice(decimal price, int providerId)
        {
            var provider = await _providerRepository.Get(providerId);
            var commission = price * provider.Commission;
            var discount = price * 0.05m;

            return price + commission - discount;
        }
    }
}
