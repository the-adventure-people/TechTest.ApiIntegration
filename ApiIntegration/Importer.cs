using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class Importer : IImporter
    {
        private readonly ITourRepository _tourRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IApiDownloader _apiDownloader;
        private readonly IMapper _mapper;
        private readonly IPricingStrategy _pricingStrategy;
        private readonly ILogger _logger;

        public Importer(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader,
            IPricingStrategy pricingStrategy,
            IMapper mapper,
            ILogger logger)
        {
            _tourRepository = tourRepository;
            _providerRepository = providerRepository;
            _apiDownloader = apiDownloader;
            _pricingStrategy = pricingStrategy;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task Execute(int providerId)
        {
            var provider = await _providerRepository.GetAsync(providerId);

            if (provider == null)
            {
                throw new ArgumentException($"Provider could not be found for Provider Id '{providerId}'.");
            }

            _logger.LogInformation($"Download Started for Provider ID: {providerId}");

            // I don't believe the downloader should be tied to a specific provider response type so I've made it generic
            var providerResponse = await _apiDownloader.DownloadAsync<ApiAvailabilityResponse>(provider.ApiEndpoint);

            // It would be worth checking the provider's status codes as they may be different from HTTP status codes, which may affect the logging/logic here
            if (providerResponse.StatusCode != 200)
            {
                _logger.LogError($"Provider returned with a status code of {providerResponse.StatusCode}.");
                return;
            }

            if (providerResponse == null)
            {
                _logger.LogInformation("No content found at the specified endpoint.");
                return;
            }

            var providerTours = providerResponse.Body
                .GroupBy(t => t.ProductCode)
                .Select(t => new
                {
                    ProductCode = t.Key,
                    Availability = t.ToList()
                });

            foreach (var providerTour in providerTours)
            {
                // It would be worthwhile considering the smallest acceptable unit of work.
                // I've assumed that updating a single tour is the smallest unit of work.
                try
                {
                    var tour = await _tourRepository.GetAsync(providerTour.ProductCode);

                    if (tour == null)
                    {
                        continue;
                    }

                    // Transform provider model to our model
                    tour.Availabilities.Clear();
                    foreach (var availability in providerTour.Availability)
                    {
                        tour.Availabilities.Add(_mapper.Map<TourAvailability>(availability, o => o.AfterMap((src, dest) => dest.TourId = tour.TourId)));
                    }

                    // Adjust prices
                    foreach (var availability in tour.Availabilities)
                    {
                        availability.SellingPrice = _pricingStrategy.CalculatePrice(provider, availability.SellingPrice);
                    }

                    // Save to repositories
                    await _tourRepository.Update(tour);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while attempting to update tour. Tour reference: '{providerTour.ProductCode}'");
                }
            }

            _logger.LogInformation("Download Finished");
        }
    }
}