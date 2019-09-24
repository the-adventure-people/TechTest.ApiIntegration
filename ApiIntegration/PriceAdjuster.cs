namespace ApiIntegration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models;

    public class PriceAdjuster : IPriceAdjuster
    {
        private readonly ILogger _logger;
        private readonly IProviderRepository _providerRepository;
        private readonly IPriceBuilder _priceBuilder;

        public PriceAdjuster(IProviderRepository providerRepository, IPriceBuilder priceBuilder, ILogger logger)
        {
            _providerRepository = providerRepository;
            _priceBuilder = priceBuilder;
            _logger = logger;
        }

        public async Task<ICollection<TourAvailability>> Adjust(ICollection<TourAvailability> tourAvailabilities,
            int providerId)
        {
            var adjustedList = new List<TourAvailability>();
            foreach (var availability in tourAvailabilities)
                try
                {
                    var provider = await _providerRepository.Get(providerId) ??
                                   throw new InvalidDataException($"Provider doesn't exist for id {providerId}");

                    availability.AdultPrice = _priceBuilder.Build(provider, availability.AdultPrice);
                    adjustedList.Add(new TourAvailability
                    {
                        AdultPrice = _priceBuilder.Build(provider, availability.AdultPrice),
                        AvailabilityCount = availability.AvailabilityCount,
                        StartDate = availability.StartDate,
                        TourDuration = availability.TourDuration,
                        TourId = availability.TourId
                    });
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Error, 1, e, e.Message);
                }

            return adjustedList;
        }
    }
}