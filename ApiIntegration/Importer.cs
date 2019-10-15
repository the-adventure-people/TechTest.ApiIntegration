using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiIntegration.Models;

namespace ApiIntegration
{
    public class Importer : IImporter
    {
        private readonly ITourRepository _tourRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IApiDownloader _apiDownloader;
        private readonly ILogger _logger;

        public Importer(ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader,
            ILogger logger)
        {
            _tourRepository = tourRepository;
            _providerRepository = providerRepository;
            _apiDownloader = apiDownloader;
            _logger = logger;
        }

        public async Task Execute(int providerId)
        {
            _logger.LogInformation("Download Started");

            var apiResponse = await _apiDownloader.Download();
            var provider = await _providerRepository.Get(providerId);

            try
            {
                if (provider != null)
                {
                    if (apiResponse.StatusCode == (int)HttpStatusCode.OK)
                    {
                        var mappings = new List<TourAvailability>();

                        var existingTours = _tourRepository.GetAllTours().Result;
                        var productCodesToTourIds = existingTours.Values.ToDictionary(t => t.TourRef, t => t.TourId);

                        // map to existing model
                        foreach (var availability in apiResponse.Body)
                        {
                            if (!apiResponse.Body.Any()) break;

                            // ignore updates for non-existent tours
                            if (!productCodesToTourIds.ContainsKey(availability.ProductCode)) break;

                            var startDateValid = DateTime.TryParse(availability.DepartureDate, out var startDate);

                            if (startDateValid)
                            {
                                var tourId = productCodesToTourIds[availability.ProductCode];

                                mappings.Add(
                                    new TourAvailability
                                    {
                                        TourId = tourId,
                                        StartDate = startDate,
                                        TourDuration = availability.Nights,
                                        AdultPrice = AdjustPrice(availability.Price, provider), //adjust price with discount and commission,
                                        AvailabilityCount = availability.Spaces
                                    });

                                var tour = _tourRepository.Get(tourId).Result;
                                tour.Availabilities = mappings.Where(mapping => mapping.TourId == tourId).ToList();

                                // save to repository
                                await _tourRepository.Update(tour);
                            }
                            else
                            {
                                _logger.LogError($"Could not parse the given date: {availability.DepartureDate}");
                            }
                        }
                    }
                    else
                    {
                        _logger.LogError($"Download failed with status code {apiResponse.StatusCode}");
                    }
                }
                else
                {
                    _logger.LogError("Provider does not exist");
                }
            }
            catch
            {
                throw new Exception("Download Request Failed");
            }

            _logger.LogInformation("Download Finished");
        }

        public static decimal AdjustPrice(decimal price, Provider provider)
        {
            var discount = 0.95m * price;
            var commission = 0.15m * price;
            return price + commission - discount;
        }
    }
}
