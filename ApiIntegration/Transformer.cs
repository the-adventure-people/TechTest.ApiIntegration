namespace ApiIntegration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Extensions;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Models;
    using ProviderModels;

    public class Transformer : ITransformer
    {
        private readonly ILogger _logger;
        private readonly ITourRepository _tourRepository;

        public Transformer(ITourRepository tourRepository, ILogger logger)
        {
            _tourRepository = tourRepository;
            _logger = logger;
        }

        public async Task<ICollection<TourAvailabilityWithTour>> Transform(ApiAvailabilityResponse apiAvailabilityResponse, int providerId)
        {
            var tourAvailabilities = new List<TourAvailabilityWithTour>();
            foreach (var a in apiAvailabilityResponse.Body)
            {
                try
                {
                    var matchingTour = await _tourRepository.Get(t => t.ProviderId == providerId
                                                                && t.TourRef == a.ProductCode) ??
                                       throw new InvalidDataException("Tour doesn't exist'");
                    
                    var newTourAvailability = new TourAvailability
                    {
                        AdultPrice = a.Price,
                        AvailabilityCount = a.Spaces,
                        StartDate = a.DepartureDate.ToDateTime(),
                        TourDuration = a.Nights,
                        TourId = matchingTour.TourId
                    };
                    tourAvailabilities.Add(new TourAvailabilityWithTour
                    {
                        Tour = matchingTour,
                        TourAvailability = newTourAvailability
                        
                    });
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Error, 1, e, e.Message);
                }
            }

            return tourAvailabilities;
        }
    }
}