using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class Importer : IImporter
    {
        private readonly ITourRepository tourRepository;
        private readonly IProviderRepository providerRepository;
        private readonly IApiDownloader apiDownloader;
        private readonly ITourPricing tourPricing;
        private readonly ILogger<Importer> logger;

        public Importer(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader,
            ITourPricing tourPricing,
            ILogger<Importer> logger)
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.apiDownloader = apiDownloader;
            this.tourPricing = tourPricing;
            this.logger = logger;
        }


        public async Task Execute(int providerId)
        {
            logger.LogInformation("Download Started");

            // Get provider
            var provider = await GetProvider(providerId);
            if (provider == null)
            {
                logger.LogError($"Provider not found by ID {providerId}. Cancelling.");
                return;
            }

            // Download
            var providerResponse = await apiDownloader.Download();

            // Handle downloaded data
            if (providerResponse.Body.Any())
                await ImportAvailabilities(new ImportAvailabilitiesRequest
                {
                    Availabilities = providerResponse.Body,
                    Provider = provider
                });

            logger.LogInformation("Download Finished");
        }




        private async Task ImportAvailabilities(ImportAvailabilitiesRequest req)
        {
            logger.LogInformation("Download complete. Importing to repositories.");

            // Get a list of tours by this provider that will be updated
            var tours = await GetToursByProvider(req.Provider.ProviderId);

            foreach (var tour in tours)
            {
                await UpdateTourAvailabilities(new UpdateTourAvailabilitiesRequest
                {
                    Tour = tour,
                    Availabilities = req.Availabilities.Where(a => a.ProductCode == tour.TourRef).ToList(),
                    Provider = req.Provider
                });
            }
        }




        private async Task UpdateTourAvailabilities(UpdateTourAvailabilitiesRequest req)
        {
            var tourAvailabilities = new List<TourAvailability>();

            foreach (var item in req.Availabilities)
            {
                DateTime.TryParseExact(item.DepartureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDate);

                // Find the price that should be saved
                decimal price = tourPricing.CalculateWebsitePrice(new CalculateWebsitePriceRequest
                {
                    ProviderPrice = item.Price,
                    ComissionPercentage = req.Provider.Commission
                });

                // Transform provider model to our model
                var existingAvailability = req.Tour.Availabilities.SingleOrDefault(a => a.StartDate.Date == departureDate.Date);
                if (existingAvailability == null)
                {
                    req.Tour.Availabilities.Add(new TourAvailability
                    {
                        TourId = req.Tour.TourId,
                        AvailabilityCount = item.Spaces,
                        SellingPrice = price,
                        StartDate = departureDate,
                        TourDuration = item.Nights
                    });
                }
                else
                {
                    existingAvailability.AvailabilityCount = item.Spaces;
                    existingAvailability.SellingPrice = price;
                    existingAvailability.StartDate = departureDate;
                    existingAvailability.TourDuration = item.Nights;
                }
            }

            // Save to repositories
            await tourRepository.Update(req.Tour);
        }




        #region Get methods
        private async Task<Provider> GetProvider(int providerId)
        {
            return await providerRepository.Get(providerId);
        }

        private async Task<List<Tour>> GetToursByProvider(int providerId)
        {
            return await tourRepository.GetByProider(providerId);
        }
        #endregion
    }
}
