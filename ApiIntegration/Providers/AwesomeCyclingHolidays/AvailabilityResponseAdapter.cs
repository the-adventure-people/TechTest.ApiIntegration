namespace ApiIntegration.Providers.AwesomeCyclingHolidays
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using ApiIntegration.Models;
    using ApiIntegration.Providers.AwesomeCyclingHolidays.Models;
    using ApiIntegration.Repositories;

    using FluentValidation;

    using Microsoft.Extensions.Logging;

    public class AvailabilityResponseAdapter : IProviderResponseAdapter<AvailabilityResponse>
    {
        private readonly ITourRepository _tourRepository;
        private readonly IValidator<Availability?> _validator;
        private readonly ILogger<AvailabilityResponseAdapter> _logger;

        public AvailabilityResponseAdapter(
            ITourRepository tourRepository,
            IValidator<Availability?> validator,
            ILogger<AvailabilityResponseAdapter> logger)
        {
            _tourRepository = tourRepository;
            _validator = validator;
            _logger = logger;
        }
        public async IAsyncEnumerable<TourAvailability> ConvertToTourAvailabilityAsync(
            AvailabilityResponse response, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var availbility in response.Body)
            {
                var validationResult = await _validator.ValidateAsync(availbility, cancellationToken);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Invalid availaibility from provider: {error}", validationResult.GetErrorMessages());
                    continue;
                }

                var tour = await _tourRepository.GetAsync(availbility.ProductCode!, cancellationToken);

                if (tour is null)
                {
                    _logger.LogWarning("Unrecognised tour found with code {productCode}", availbility.ProductCode);
                    continue;
                }

                yield return new TourAvailability
                { 
                    TourID = tour.TourID,
                    StartDate = availbility.DepartureDate,
                    TourDuration = availbility.Nights,
                    Cost = availbility.Price,
                    AvailabilityCount = availbility.Spaces
                };
            }
        }
    }
}
