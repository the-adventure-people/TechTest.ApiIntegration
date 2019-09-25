
using ApiIntegration.Infrastructure.Repositories.Tour;
using ApiIntegration.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration.Infrastructure.Repositories.TourRepository
{
    public class TourRepository : ITourRepository
    {
        private readonly Dictionary<int, TourEntity> tours;

        public TourRepository()
        {
            tours = new Dictionary<int, TourEntity>()
            {
                { 1, new TourEntity()
                    {
                        TourId = 1,
                        TourRef = "EUR123",
                        TourName = "Cycling Danube",
                        ProviderId = 1,
                        Active = true,
                        ReviewCount = 13,
                        ReviewScore = 4.3m,
                        Availabilities = new List<TourAvailabilityEntity>()
                        {
                            new TourAvailabilityEntity()
                            {
                                TourId = 1,
                                AdultPrice = 500,
                                StartDate = new DateTime(2020, 6, 20),
                                TourDuration = 6,
                                AvailabilityCount = 9
                            },
                            new TourAvailabilityEntity()
                            {
                                TourId = 1,
                                AdultPrice = 450,
                                StartDate = new DateTime(2020, 6, 27),
                                TourDuration = 6,
                                AvailabilityCount = 9
                            }
                        }
                    } },
                { 2, new TourEntity()
                    {
                        TourId = 2,
                        TourRef = "EUR456",
                        TourName = "Cycling Rhine",
                        ProviderId = 1,
                        Active = true,
                        ReviewCount = 55,
                        ReviewScore = 4.8m,
                        Availabilities = new List<TourAvailabilityEntity>()
                        {
                            new TourAvailabilityEntity()
                            {
                                TourId = 2,
                                AdultPrice = 720,
                                StartDate = new DateTime(2020, 3, 10),
                                TourDuration = 11,
                                AvailabilityCount = 4
                            },
                            new TourAvailabilityEntity()
                            {
                                TourId = 2,
                                AdultPrice = 720,
                                StartDate = new DateTime(2020, 3, 20),
                                TourDuration = 11,
                                AvailabilityCount = 5
                            }
                        }
                    }
                }
            };
        }

        public Task<TourEntity> Get(int tourId, string tourRef)
        {
            TourEntity tour;
            if (tourId != default && tours.ContainsKey(tourId))
            {
                tour = tours[tourId];
            }
            else if (!string.IsNullOrWhiteSpace(tourRef))
            {
                tour = tours.Values
                    .SingleOrDefault(t => t.TourRef.Equals(tourRef, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                tour = null;
            }

            return Task.FromResult(tour);
        }

        public Task<TourEntity> GetByTourRef(string tourRef)
        {
            TourEntity tour;
             if (!string.IsNullOrWhiteSpace(tourRef))
            {
                tour = tours.Values
                    .SingleOrDefault(t => t.TourRef.Equals(tourRef, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                tour = null;
            }

            return Task.FromResult(tour);
        }

        public Task<List<TourEntity>> GetByTourRefs(string[] tourRefs)
        {
            List<TourEntity> toursToReturn;

            if (tourRefs.Length > 0)
            {
                toursToReturn = tours.Values
                    .Where(t => tourRefs.Any(tourRef => tourRef == t.TourRef)).ToList();
            }
            else
            {
                toursToReturn = null;
            }

            return Task.FromResult(toursToReturn);
        }

        public Task Update(TourEntity tour)
        {
            if (tour.TourId != default
                    && tours.ContainsKey(tour.TourId))
            {
                tours[tour.TourId] = tour;
            }
            else
            {
                throw new Exception($"Tour with TourId: {tour.TourId} does not exist");
            };

            return Task.CompletedTask;
        }
    }
}
