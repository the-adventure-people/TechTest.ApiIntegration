namespace ApiIntegration.Repositories
{
    using ApiIntegration.Models;

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class TourRepository : ITourRepository
    {
        private readonly ConcurrentDictionary<int, Tour> _tourIDLookup;

        public TourRepository()
        {
            var tours = new List<Tour>
            {
                new Tour
                {
                    TourID = 1,
                    TourRef = "EUR123",
                    TourName = "Cycling Danube",
                    ProviderId = 1,
                    Active = true,
                    ReviewCount = 13,
                    ReviewScore = 4.3m,
                    Availabilities = new List<PricedTourAvailability>
                    {
                        new PricedTourAvailability
                        {
                            TourId = 1,
                            SellingPrice = 500,
                            StartDate = new DateTime(2020, 6, 20),
                            TourDuration = 6,
                            AvailabilityCount = 9
                        },
                        new PricedTourAvailability
                        {
                            TourId = 1,
                            SellingPrice = 450,
                            StartDate = new DateTime(2020, 6, 27),
                            TourDuration = 6,
                            AvailabilityCount = 9
                        }
                    }
                },
                new Tour
                {
                    TourID = 2,
                    TourRef = "EUR456",
                    TourName = "Cycling Rhine",
                    ProviderId = 1,
                    Active = true,
                    ReviewCount = 55,
                    ReviewScore = 4.8m,
                    Availabilities = new List<PricedTourAvailability>
                    {
                        new PricedTourAvailability
                        {
                            TourId = 2,
                            SellingPrice = 720,
                            StartDate = new DateTime(2020, 3, 10),
                            TourDuration = 11,
                            AvailabilityCount = 4
                        },
                        new PricedTourAvailability
                        {
                            TourId = 2,
                            SellingPrice = 720,
                            StartDate = new DateTime(2020, 3, 20),
                            TourDuration = 11,
                            AvailabilityCount = 5
                        }
                    }
                }
            };

            _tourIDLookup = new ConcurrentDictionary<int, Tour>(tours.ToDictionary(x => x.TourID));
        }

        public Task<Tour?> GetAsync(string tourReference, CancellationToken cancellationToken = default) =>
            Task.FromResult(!string.IsNullOrWhiteSpace(tourReference)
                ? _tourIDLookup.Values.SingleOrDefault(t => t.TourRef.Equals(tourReference, StringComparison.OrdinalIgnoreCase))
                : null);

        public Task<Tour?> GetAsync(int tourId, CancellationToken cancellationToken = default) =>
            Task.FromResult(_tourIDLookup.TryGetValue(tourId, out var tour)
                ? tour
                : null);

        public Task<bool> UpdateAsync(Tour tour, CancellationToken cancellationToken = default) =>
            Task.FromResult(tour.TourID != default
                && _tourIDLookup.TryGetValue(tour.TourID, out var currentTour)
                && _tourIDLookup.TryUpdate(tour.TourID, tour, currentTour));
    }
}
