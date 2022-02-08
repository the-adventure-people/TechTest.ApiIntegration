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
        private readonly ILogger<Importer> logger;

        public Importer(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader,
            ILogger<Importer> logger)
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.apiDownloader = apiDownloader;
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
                await UpdateTourAvailabilities(tour, req.Availabilities.Where(a => a.ProductCode == tour.TourRef).ToList());
            }
        }




        private async Task UpdateTourAvailabilities(Tour tour, List<Availability> availabilities)
        {
            var tourAvailabilities = new List<TourAvailability>();

            foreach (var item in availabilities)
            {
                DateTime.TryParseExact(item.DepartureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureDate);

                // TODO Adjust prices

                // Transform provider model to our model
                var existingAvailability = tour.Availabilities.SingleOrDefault(a => a.StartDate.Date == departureDate.Date);
                if (existingAvailability == null)
                {
                    tour.Availabilities.Add(new TourAvailability
                    {
                        TourId = tour.TourId,
                        AvailabilityCount = item.Spaces,
                        SellingPrice = item.Price,
                        StartDate = departureDate,
                        TourDuration = item.Nights
                    });
                }
                else
                {
                    existingAvailability.AvailabilityCount = item.Spaces;
                    existingAvailability.SellingPrice = item.Price; 
                    existingAvailability.StartDate = departureDate;
                    existingAvailability.TourDuration = item.Nights;
                }
            }

            // Save to repositories
            await tourRepository.Update(tour);
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

// Transform provider model to our model

// Adjust prices

// Save to repositories


/*
foreach (var item in req.Availabilities)
{
    var tour = await GetTour(req.Provider.ProviderId, item.ProductCode);
    if (tour == null)
    {
        logger.LogError($"Could not find tour by id {req.Provider.ProviderId} and code {item.ProductCode}. Skipping item.");
        continue;
    }

    await UpdateTourAvailabilities(tour, item);
}
*/