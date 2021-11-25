using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class TourRepository : ITourRepository
    {
        private readonly Dictionary<int, Tour> _tours;

        public TourRepository()
        {
            _tours = new Dictionary<int, Tour>()
            {
                { 1, new Tour()
                    {
                        TourId = 1,
                        TourRef = "EUR123",
                        TourName = "Cycling Danube",
                        ProviderId = 1,
                        Active = true,
                        ReviewCount = 13,
                        ReviewScore = 4.3m,
                        Availabilities = new List<TourAvailability>()
                        {
                            new TourAvailability()
                            {
                                TourId = 1,
                                SellingPrice = 500,
                                StartDate = new DateTime(2020, 6, 20),
                                TourDuration = 6,
                                AvailabilityCount = 9
                            },
                            new TourAvailability()
                            {
                                TourId = 1,
                                SellingPrice = 450,
                                StartDate = new DateTime(2020, 6, 27),
                                TourDuration = 6,
                                AvailabilityCount = 9
                            }
                        }
                    } },
                { 2, new Tour()
                    {
                        TourId = 2,
                        TourRef = "EUR456",
                        TourName = "Cycling Rhine",
                        ProviderId = 1,
                        Active = true,
                        ReviewCount = 55,
                        ReviewScore = 4.8m,
                        Availabilities = new List<TourAvailability>()
                        {
                            new TourAvailability()
                            {
                                TourId = 2,
                                SellingPrice = 720,
                                StartDate = new DateTime(2020, 3, 10),
                                TourDuration = 11,
                                AvailabilityCount = 4
                            },
                            new TourAvailability()
                            {
                                TourId = 2,
                                SellingPrice = 720,
                                StartDate = new DateTime(2020, 3, 20),
                                TourDuration = 11,
                                AvailabilityCount = 5
                            }
                        }
                    }
                }
            };
        }

        public Task<Tour> GetAsync(int tourId)
        {
            var tour = tourId != default && _tours.ContainsKey(tourId)
                ? _tours[tourId]
                : null;

            return Task.FromResult(tour);
        }

        public Task<Tour> GetAsync(string tourRef)
        {
            var tour = !string.IsNullOrWhiteSpace(tourRef)
                ? _tours.Values.SingleOrDefault(t => t.TourRef.Equals(tourRef, StringComparison.OrdinalIgnoreCase))
                : null;

            return Task.FromResult(tour);
        }

        public Task Update(Tour tour)
        {
            if (tour.TourId != default
                && _tours.ContainsKey(tour.TourId))
            {
                _tours[tour.TourId] = tour;
            }
            else
            {
                throw new Exception($"Tour with TourId: {tour.TourId} does not exist");
            };

            return Task.CompletedTask;
        }
    }
}
