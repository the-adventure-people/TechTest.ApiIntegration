using ApiIntegration.Interfaces;
using ApiIntegration.Models;
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
            ILogger logger)
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.apiDownloader = apiDownloader;
            this.logger = logger;
        }


        public async Task Execute(int providerId, decimal discountDecimal)
        {
            logger.LogInformation("Download Started");

            try
            {
                var providerResponse = await apiDownloader.Download();
                var providermodel = providerResponse.Body;
                var avalibilities = new List<TourAvailability>();
                var historictourdata = new List<Tour>();

                // Transform provider model to our model
                foreach (var obj in providermodel)
                {
                    Tour currenttour = historictourdata.FirstOrDefault(h => h.TourRef == obj.ProductCode);
                    if (currenttour == null)
                    {
                        currenttour = await tourRepository.Get(default, obj.ProductCode);
                        if (currenttour == null)
                        {
                            logger.LogWarning("Tour with product code " + obj.ProductCode + " not found in DB.");

                            continue;
                        }
                        historictourdata.Add(currenttour);
                        currenttour.Availabilities = new List<TourAvailability>();
                    }
                    currenttour.Availabilities.Add(new TourAvailability
                    {
                        TourId = currenttour.TourId,
                        StartDate = DateTime.Parse(obj.DepartureDate),
                        TourDuration = obj.Nights,
                        AvailabilityCount = obj.Spaces,
                        SellingPrice = obj.Price
                    });
                }

                // Adjust prices
                // Save to repositories
                var providerInfo = await providerRepository.Get(providerId);
                foreach (var data in historictourdata)
                {
                    foreach (var a in data.Availabilities)
                    {
                        a.SellingPrice = await AdjustPrice(a.SellingPrice, providerId, discountDecimal);
                    }
                    await tourRepository.Update(data);
                }

            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred. Message: " + ex.Message + "\n" +  ex.StackTrace);
            }
            logger.LogInformation("Download Finished");
        }

        private async Task<decimal> AdjustPrice(decimal price, int providerid, decimal discountDecimal)
        {
            logger.LogInformation(" Prices adjusted by " + discountDecimal * 100 + " percent.");
            var providerInfo = await providerRepository.Get(providerid);
            return price + price * providerInfo.Commission - price * discountDecimal;
        }
    }
}
