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


        public async Task Execute(int providerId)
        {
            try
            {
                var provider = await providerRepository.Get(providerId); // Get provider for the tours we're importing for

                if(provider == null)
                {
                    throw new Exception($"No Provider found for id: { providerId }");
                }

                logger.LogInformation("Download Started");
                var providerResponse = await apiDownloader.Download();
                var tours = await tourRepository.Get(g => g.ProviderId == 1 && providerResponse.Body.Select(s => s.ProductCode).Any(a => a == g.TourRef)); // Get only tours that we're importing for
                
                var importList = new List<TourAvailability>();

                // Transform provider model to our model
                foreach (var availability in providerResponse.Body)
                {
                    // We only care about availability for tours we already have
                    if (tours.Any(a => a.TourRef == availability.ProductCode))
                    {
                        var ta = availability.ToTourAvailability();
                        ta.TourId = tours.First(f => f.TourRef == availability.ProductCode).TourId;

                        importList.Add(ta);
                    }
                    else
                    {
                        logger.LogInformation($"Availability found for a tour, with ref: { availability.ProductCode }, not currently in the system for Provider: { provider.Name }");
                    }
                }

                // Adjust prices
                foreach (var item in importList)
                {
                    var tour = tours.First(f => f.TourId == item.TourId);
                    item.SellingPrice = await AdjustPrice(provider, item.SellingPrice);

                    tour.Availabilities.Add(item);
                }

                // Save to repositories
                foreach (var tour in tours.ToList())
                {
                    await tourRepository.Update(tour);
                }

                logger.LogInformation("Download Finished");
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }

        /// <summary>
        /// Adjust the price of the tour availability according to the associated provider's commision and applying discount where appropriate
        /// </summary>
        /// <param name="provider">The Provider of the tour</param>
        /// <param name="price">The original price supplied to us</param>
        /// <returns>Adjusted price including commission and applied discount</returns>
        private async Task<decimal> AdjustPrice(Provider provider, decimal price)
        {
            var newPrice = price + (price * provider.Commission);

            if(AssemblySettings.ApplyDiscount)
            {
                // It was a little unclear to me about whether the discount percentage was applied to the original price or the price after commission.
                // My understanding was to apply it to the original price.
                newPrice = newPrice - (price * AssemblySettings.Discount);
            }

            return newPrice;
        }
    }
}
