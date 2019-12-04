using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using ApiIntegration.Models;
using System.Collections.Generic;
using ApiIntegration.ProviderModels;

namespace ApiIntegration
{
    public class Importer : IImporter
    {
        private readonly ITourRepository tourRepository;
        private readonly IProviderRepository providerRepository;
        private readonly IApiDownloader apiDownloader;
        private readonly IPricingStrategy pricingStrategy;
        private readonly ILogger<Importer> logger;

        public Importer(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader,
            IPricingStrategy pricingStrategy,
            ILogger<Importer> logger
           )
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.apiDownloader = apiDownloader;
            this.pricingStrategy = pricingStrategy;
            this.logger = logger;
        }

        public async Task Execute(int providerId)
        {
            logger.LogInformation($"Download for providerId '{providerId}' started");

            var providerInfo = await providerRepository.Get(providerId);
            if (providerInfo == null)
            {
                var errorMessage = $"A provider with Id '{providerId}' could not be found";
                logger.LogError(errorMessage);
                throw new KeyNotFoundException(errorMessage);
            }

            var providerResponse = await apiDownloader.Download(providerInfo.Url);

            // Transform provider model to our model
            var availabilityByProduct = providerResponse.Body.GroupBy(x => x.ProductCode);
            foreach (var productAvailabilites in availabilityByProduct)
            {
                var tour = await tourRepository.Get(default, productAvailabilites.Key);
                
                // We only want to update existing tour, so skip any that don't already exist
                if (tour == null) continue;

                var newTourAvailabilities = new List<TourAvailability>();
                foreach (var productAvailability in productAvailabilites)
                {
                    var newTourAvailability = MapAvailability(tour.TourId, productAvailability);
                    
                    newTourAvailability.AdultPrice = pricingStrategy.AdjustPrice(newTourAvailability.AdultPrice, 
                        providerInfo.Discount, providerInfo.Commission);

                    newTourAvailabilities.Add(newTourAvailability);
                }

                tour.Availabilities = newTourAvailabilities;
                await tourRepository.Update(tour);
            }

            logger.LogInformation($"Download for providerId '{providerId}' Finished");
        }

        private TourAvailability MapAvailability(int tourId, Availability availability)
        {
            return new TourAvailability
            {
                TourId = tourId,
                StartDate = availability.DepartureDate,
                TourDuration = availability.Nights,
                AdultPrice = availability.Price,
                AvailabilityCount = availability.Spaces
            };
        }
    }
}
