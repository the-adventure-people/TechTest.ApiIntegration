using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class Importer : IImporter
    {
        private ITourRepository TourRepository { get; }
        private IProviderRepository ProviderRepository { get; }
        private IApiDownloader ApiDownloader { get; }
        private ILogger<Importer> Logger { get; }
        private IPriceHandler PriceHandler { get; }

        public Importer(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader, 
            ILogger<Importer> logger,
            IPriceHandler priceHandler)
        {
            TourRepository = tourRepository;
            ProviderRepository = providerRepository;
            ApiDownloader = apiDownloader;
            Logger = logger;
            PriceHandler = priceHandler;
        }
        public async Task Execute(int providerId)
        {
            Logger.LogInformation("Download Started");

            ApiAvailabilityResponse toursResponse;
            try {
                toursResponse = await ApiDownloader.Download().ConfigureAwait(false);
            }
            catch (HttpRequestException exception) {
                Logger.LogError(exception, $"An error occured during the request.");
                throw;
            }
            catch (Exception exception) {
                Logger.LogError(exception, "An error has occured");
                throw;
            }
            Logger.LogInformation("Download Finished");

            Logger.LogInformation("Calculating discount...");
            var tours = toursResponse.Body;
            var provider = await ProviderRepository.Get(providerId).ConfigureAwait(false);
            CalculateNewPrice(provider, tours);

            Logger.LogInformation("Mapping to DB entity models...");
            var dbEntityToursDict = MapModels(tours);

            Logger.LogInformation("Adding To Database...");
            foreach (var entityGroupings in dbEntityToursDict) {
                foreach(var entity in entityGroupings){
                    await TourRepository.AddTourAvailability(entity, entityGroupings.Key);
                }
            }

            var allTours = await TourRepository.GetAll();
            Logger.LogInformation($"Tours after update: {JsonConvert.SerializeObject(allTours, Formatting.Indented)}");

            Logger.LogInformation("Finished adding new tours");

        }

        private IEnumerable<IGrouping<string, TourAvailability>> MapModels(List<Availability> availabilities){
            return availabilities.GroupBy(x => x.ProductCode, y => y.ToTourAvailability());
        }

        private void CalculateNewPrice(Provider provider, List<Availability> tours){ 
            foreach(var t in tours){
                t.Price = PriceHandler.GetPrice(t.Price, provider.Commission);
            }
        }
    }
}
