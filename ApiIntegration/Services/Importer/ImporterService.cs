using ApiIntegration.Data.Models;
using ApiIntegration.Data.Models.External;
using ApiIntegration.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class ImporterService : IImporterService
    {
        private readonly ITourService _tourService;
        private readonly IApiDownloaderService<ApiAvailabilityResponse> _apiDownloaderService;
        private readonly IFinanceService _financeService;

        private readonly ILogger<IImporterService> _logger;

        public ImporterService(
            ITourService tourService,
            IApiDownloaderService<ApiAvailabilityResponse> apiDownloaderService,
            IFinanceService financeService, 
            ILogger<IImporterService> logger)
        {
            _tourService = tourService;
            _apiDownloaderService = apiDownloaderService;
            _financeService = financeService;
            _logger = logger;
        }

        public async Task ExecuteAsync(int providerId)
        {
            var startTime = DateTime.Now;

            try
            {
                // TODO: Use providerId to look up which implementation of the IAPIDownloadServer to use
                var availabilityResponse = await _apiDownloaderService.DownloadAsync();

                var transformedResponse = await TransformProviderResponseAsync(availabilityResponse);

                await SaveImportResultAsync(transformedResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(Events.ImporterService, ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation(Events.ImporterService, "{0}", Helpers.Duration(startTime));
            }
        }

        private async Task<IEnumerable<TourAvailability>> TransformProviderResponseAsync(ApiAvailabilityResponse providerResponse)
        {
            List<TourAvailability> availabilities = new List<TourAvailability>();

            var startTime = DateTime.Now;

            try
            {
                foreach (var providerAvailability in providerResponse.Body)
                {
                    var savedTour = await _tourService.GetAsync(providerAvailability.ProductCode);

                    if (!(savedTour is null))
                    {
                        availabilities.Add(await MapProviderAvailabilityAsync(providerAvailability, savedTour));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(Events.ImporterService, ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation(Events.ImporterService, "{0}", Helpers.Duration(startTime));
            }

            return availabilities;
        } 

        private async Task<TourAvailability> MapProviderAvailabilityAsync(Availability providerAvailability, Tour savedTour)
        {
            return new TourAvailability()
            {
                AvailabilityCount = providerAvailability.Spaces,
                StartDate = providerAvailability.DepartureDate,
                SellingPrice = await _financeService.SetSellingPriceAsync(providerAvailability.Price, savedTour.ProviderId),
                TourDuration = providerAvailability.Nights,
                TourId = savedTour.TourId
            };
        }

        private async Task SaveImportResultAsync(IEnumerable<TourAvailability> availabilities)
        {
            await _tourService.UpdateToursUsingTourAvailabilities(availabilities);
        }
    }
}
