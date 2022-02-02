namespace ApiIntegration.Services
{
    using ApiIntegration.Interfaces;
    using ApiIntegration.Models;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public class Importer : IImporter
    {
        private readonly ITourRepository _tourRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IApiDownloader _apiDownloader;
        private readonly IPricingService _pricingService;
        private readonly ILogger _logger;

        public Importer(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader,
            IPricingService pricingService,
            ILogger logger)
        {
            _tourRepository = tourRepository;
            _providerRepository = providerRepository;
            _apiDownloader = apiDownloader;
            _pricingService = pricingService;
            _logger = logger;
        }

        public async Task ExecuteAsync(int providerId)
        {
            _logger.LogInformation("Download Started...");

            try
            {
                var providerResponse = await _apiDownloader.Download();
                var provider = await _providerRepository.Get(providerId);
                var numImported = 0;

                // Store availability
                foreach (var availability in providerResponse.Body)
                {
                    var tour = await _tourRepository.Get(providerId, availability.ProductCode);
                    DateTime.TryParseExact(availability.DepartureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newAvailStartDate);

                    if (tour != null)
                    {
                        if (!tour.Availabilities.Any(avail => avail.StartDate.Date == newAvailStartDate.Date))
                        {
                            // Transform provider model to our model
                            var tourAvailability = new TourAvailability
                            {
                                TourId = tour.TourId,
                                StartDate = newAvailStartDate,
                                TourDuration = availability.Nights,
                                AvailabilityCount = availability.Spaces
                            };

                            // Adjust prices
                            tourAvailability.SellingPrice = _pricingService.CalcSellingPrice(provider.Commission, availability.Price);

                            // Save to repositories
                            tour.Availabilities.Add(tourAvailability);
                            await _tourRepository.Update(tour);
                            numImported++;

                            _logger.LogInformation($"Imported Tour availability for {tourAvailability.StartDate:d}, {tourAvailability.TourDuration} nights.");
                        }
                    }
                }

                _logger.LogInformation($"{numImported} record(s) imported.");
                _logger.LogInformation("Download Finished...");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred during import.");
            }
        }
    }
}
