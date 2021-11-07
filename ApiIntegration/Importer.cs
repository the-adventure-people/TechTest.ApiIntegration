using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;

namespace ApiIntegration
{
    public class Importer : IImporter
    {
        private const decimal discount = 0.05m;

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
            logger.LogInformation("Download Started");

            ApiAvailabilityResponse providerResponse;
            try
            {
                providerResponse = await apiDownloader.Download();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Download Failed Due to Api Downloader Exception");
                return;
            }

            if (providerResponse.StatusCode != 200)
            {
                logger.LogError(new EventId(providerResponse.StatusCode), "Download Failed Due to Request Error");
                return;
            }

            // Transform provider model to our model
            var provider = await providerRepository.Get(1);

            var toursAndAvailabilityTasks = providerResponse.Body
                .Select(x => new {availability = x, tourCode = x.ProductCode})
                .GroupBy(x => x.tourCode,
                    (key, g) => new {tourCode = key, availability = g.Select(x => x.availability)})
                .Select(async x => new
                    {availability = x.availability, tour = await tourRepository.Get(default, x.tourCode)}).ToList();
            await Task.WhenAll(toursAndAvailabilityTasks);
            var toursAndAvailability = toursAndAvailabilityTasks
                .Select(x => new {availability = x.Result.availability, tour = x.Result.tour})
                .Where(x => x.tour != null)
                .Select(x => new
                {
                    tour = x.tour, availability = x.availability.Select(y => new TourAvailability
                    {
                        AvailabilityCount = y.Spaces,
                        SellingPrice = AdjustPrice(y.Price, provider.Commission, discount),
                        StartDate = ParseDate(y.DepartureDate),
                        TourDuration = y.Nights,
                        TourId = x.tour.TourId
                    })
                    .Where(y => y.StartDate != DateTime.MinValue)
                    .ToList()
                }).ToList();
            toursAndAvailability.ForEach(x => x.tour.Availabilities = x.availability);
            var tours = toursAndAvailability.Select(x => x.tour).ToList();

            // Save to repositories
            tours.ForEach(x => tourRepository.Update(x));

            logger.LogInformation("Download Finished");
        }

        private DateTime ParseDate(string date)
        {
            DateTime result;
            if (DateTime.TryParse(date, out result))
                return result;
            else
                return DateTime.MinValue;

        }

        private decimal AdjustPrice(decimal price, decimal comission, decimal discount)
        {
            return (1 + comission - discount) * price;
        }
    }
}
