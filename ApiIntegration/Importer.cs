using ApiIntegration.Extensions;
using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class Importer : IImporter
    {
        private readonly ITourRepository tourRepository;
        private readonly IProviderRepository providerRepository;
        private readonly IApiDownloader apiDownloader;
        private readonly ILogger logger;

        public Importer(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader,
            ILoggerFactory loggerFactory)
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.apiDownloader = apiDownloader;
            this.logger = loggerFactory.CreateLogger<Importer>();
        }

        public async Task ExecuteAsync(int providerId)
        {
            logger.LogInformation("Download Started");

            var providerResponse = await apiDownloader.DownloadAsync();

            // Transform provider model to our model
            List<TourAvailability> tourAvailabilities = new List<TourAvailability>();
            foreach (Availability availability in providerResponse.Body)
            {
                Tour tour = await tourRepository.GetTourAsync(providerId, availability.ProductCode);

                if (tour != null)
                    tourAvailabilities.Add(availability.ToTourAvailability(tour.TourId));
            };

            // Adjust prices
            // These should really be adjusted when displaying them to the customer, not stored in the database.
            // What happens when the sale ends???
            // TODO: discount as setting
            Provider provider = await providerRepository.GetAsync(providerId);

            if (provider == null)
            {
                logger.LogError("Provider with ID of {@ID} was not found in repository", providerId);
            }
            else
            {
                logger.LogInformation("Adjusting prices");
                tourAvailabilities
                    .Select(x => { 
                        x.SellingPrice = AdjustPrice(x.SellingPrice, provider.Commission, 0.05m);
                        return x; })
                    .ToList();

                // Save to repositories
                // We're only updating/inserting tour availability, not creating new tours
                // 4. Skip any availability data where no tour exists in our database without crashing the program
                logger.LogInformation("Saving to repository");
                foreach (TourAvailability tourAvailability in tourAvailabilities)
                {
                    await tourRepository.UpdateTourAvailabilityAsync(tourAvailability);
                };
            }

            logger.LogInformation("Download Finished");
        }

        private decimal AdjustPrice(decimal providerPrice, decimal commission, decimal discount = 0)
        {
            return providerPrice + (providerPrice * commission) - (providerPrice * discount);
        }
    }
}
