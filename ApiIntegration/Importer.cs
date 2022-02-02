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

        public const decimal SITE_DISCOUNT = 0.05M;

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

        /// <summary>
        /// Adjusts the price for available tours
        /// </summary>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public async Task Execute(int providerId, decimal discount)
        {
            Logger.LogInformation("Download Started");

            ApiAvailabilityResponse toursResponse;

            try {
                toursResponse = await ApiDownloader.Download().ConfigureAwait(false);
            }
            catch (HttpRequestException exception) {
                Logger.LogError(exception, $"Failed to get tours response");
                throw;
            }
            catch (Exception exception) {
                Logger.LogError(exception, "An error has occured");
                throw;
            }
            Logger.LogInformation("Download Finished");

            var tours = await GetTours(providerId, toursResponse);

            //Logger.LogInformation($"Tours before update: {JsonConvert.SerializeObject(tours, Formatting.Indented)}\n");

            foreach (var tour in tours){
                foreach(var availability in tour.Availabilities){
                    var provider = await ProviderRepository.Get(tour.ProviderId);
                    var newPrice = PriceHandler.GetPrice(availability.SellingPrice, provider.Commission, discount);
                    availability.SellingPrice = newPrice;
                }
                await TourRepository.UpdateTourAvailability(tour.ProviderId, tour.TourId, tour.Availabilities);
            }

            tours = await GetTours(providerId, toursResponse);

            Logger.LogInformation($"Tours after update: {JsonConvert.SerializeObject(tours, Formatting.Indented)}");

            Logger.LogInformation("Finished updating price for tours");

        }

        private async Task<List<Tour>> GetTours(int providerId, ApiAvailabilityResponse apiAvailabilityResponse){
            var tours = new List<Tour>();

            var productCodes = apiAvailabilityResponse.Body.Select(t => t.ProductCode).Distinct();
            foreach (var code in productCodes) {
                var tour = await TourRepository.Get(providerId, code);
                if (tour != null){
                    tours.Add(tour);
                }
            }
            return tours;
        }
    }
}
