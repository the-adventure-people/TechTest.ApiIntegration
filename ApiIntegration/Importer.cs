using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            ILogger logger)
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.apiDownloader = apiDownloader;
            this.logger = logger;
        }


        public async Task Execute(int providerId)
        {
            Provider provider = await providerRepository.Get(providerId);
            if (provider == null)
            {
                Exception providerNotFound = new Exception($"Failed to find provider with matching id: {providerId}");
                logger.LogError(providerNotFound.Message);
                throw providerNotFound;
            }

            // Transform provider model to our model
            List<Availability> availabilities = null;
            logger.LogInformation("Download Started");
            try
            {
                var providerResponse = await apiDownloader.Download();
                availabilities = providerResponse.Body;
            }
            catch (Exception e)
            {
                logger.LogError("Failed to download data");
                throw e;
            }


            Dictionary<string, Tour> toursToUpdate = new Dictionary<string, Tour>();
            foreach (var a in availabilities)
            {
                Tour tour;
                if (!toursToUpdate.TryGetValue(a.ProductCode, out tour))
                {
                    tour = await tourRepository.Get(0, a.ProductCode);
                    if (tour == null) continue;

                    toursToUpdate.Add(tour.TourRef, tour);
                }

                tour.Availabilities.Add(new TourAvailability()
                {
                    TourId = tour.TourId,
                    StartDate = DateTime.Parse(a.DepartureDate),
                    TourDuration = a.Nights,
                    AvailabilityCount = a.Spaces,
                    AdultPrice = await AdjustPrice(a.Price, provider.Commission)
                });
            }

            // Save to repositories
            foreach (Tour t in toursToUpdate.Values)
            {
                await tourRepository.Update(t);
            }


            logger.LogInformation("Download Finished");
        }

        private async Task<decimal> AdjustPrice(decimal price, decimal commissionPer)
        {
            decimal discount = price * 0.05m;
            decimal commission = price * commissionPer;

            return price - discount + commission;
        }
    }
}
