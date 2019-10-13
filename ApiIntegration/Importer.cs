using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;

namespace ApiIntegration
{
    public class Importer : IImporter
    {
        private readonly ITourRepository _tourRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IApiDownloader _apiDownloader;
        private readonly ILogger _logger;

        public Importer(
            ITourRepository tourRepository,
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

            try
            {
                var providerResponse = await _apiDownloader.Download();
                if (providerResponse.StatusCode == (int)HttpStatusCode.OK)
                {
                    var provider = await _providerRepository.Get(providerId);
                    if (provider == null)
                    {
                        _logger.LogError("Provider not found");
                    }
                    else
                    {

                        //4. Skip any availability data where no tour exists in our database without crashing the program
                        // when ProductCode in availabilities  is equal to _tourRepository it looks in the repository by tourref tourRepository.FindByTourRef(p.Key)))
                        //in case we have no such tourRef the method returns null
                        var availabilities = providerResponse.Body;
                        var tours = (
                                await Task.WhenAll(availabilities
                                    .Select(p => p.ProductCode)
                                    .GroupBy(p => p)
                                    //.Select(p => _tourRepository.Get(0, p.Key)))
                                    .Select(p => _tourRepository.FindByTourRef(p.Key)))
                            )
                            .Where(p => p != null)
                            .ToArray();

                        var refToIds = tours.ToDictionary(p => p.TourRef, p => p.TourId);

                        // Transform provider model to our model
                        var tourAvailabilities = availabilities
                            .Where(p => refToIds.ContainsKey(p.ProductCode))
                            .Select(p => ToTourAvailability(p, refToIds))
                            .ToArray();

                        // Adjust prices
                        foreach (var tourAvailability in tourAvailabilities)
                        {
                            //tourAvailability.AdultPrice = await AdjustPrice(tourAvailability.AdultPrice, provider);
                            tourAvailability.AdultPrice = AdjustPriceSimple(tourAvailability.AdultPrice, provider);
                        }

                        // Save to repositories
                        var tourToAvailabilities = tourAvailabilities
                            .GroupBy(p => p.TourId)
                            .ToDictionary(p => p.Key, p => p.ToList());
                        foreach (var tour in tours)
                        {
                            tour.Availabilities = tourToAvailabilities[tour.TourId];
                            await _tourRepository.Update(tour);
                        }
                    }
                }
                else
                {
                    _logger.LogError($"Error happened during downloading. Response status code {providerResponse.StatusCode}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in method Execute");
            }

            _logger.LogInformation("Download Finished");
        }

        private TourAvailability ToTourAvailability(Availability availability, Dictionary<string, int> refToIds)
        {
            var dateIsCorrect = DateTime.TryParseExact(availability.DepartureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime startDate);

            if (dateIsCorrect)
            {
                TourAvailability res = new TourAvailability()
                {
                    TourId = refToIds[availability.ProductCode],
                    StartDate = startDate,
                    TourDuration = availability.Nights,
                    AdultPrice = availability.Price,
                    AvailabilityCount = availability.Spaces
                };
                return res;
            }
            else
            {
                _logger.LogError($"Date {availability.DepartureDate} is invalid");
                return null;
            }
        }

        //For such simple function there is no reason to make it asynchronous.
        //It is better to use AdjustPriceSimple instead.
        private Task<decimal> AdjustPrice(decimal price, Provider provider)
        {
            return Task.FromResult(AdjustPriceSimple(price, provider));
        }

        private decimal AdjustPriceSimple(decimal price, Provider provider)
        {
            return price * 0.95m + provider.Commission;
        }
    }
}
