using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class TourRepository : ITourRepository
    {
        private readonly Dictionary<int, Tour> tours;

        public ILogger<TourRepository> Logger { get; }

        public TourRepository(Microsoft.Extensions.Logging.ILogger<TourRepository> logger) {
            Logger = logger;
        }
        public TourRepository()
        {
            this.tours = new Dictionary<int, Tour>() //
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

        public Task<Tour> Get(int providerId, string tourRef)
        {
            Tour tour = null;
            if (providerId != default && this.tours.ContainsKey(providerId))
            {
                if (tours[providerId].TourRef.Equals(tourRef, StringComparison.OrdinalIgnoreCase)){
                    tour = this.tours[providerId];
                }
            }

            return Task.FromResult(tour);
        }

        public Task Update(Tour tour)
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

        //in a real scenario i would expect touravailability to have a primary key that I can use to update the selling price 
        //instead of just updating the list
        public Task UpdateTourAvailability(int providerId, int tourId, List<TourAvailability> tourAvailabilities) {

            if (tours.ContainsKey(providerId)) {
                var tour = tours[providerId];

                //provider id is the same as tour ID here, i would assume one provider should have multiple tours possible
                if (tour.TourId == tourId){
                    tour.Availabilities = tourAvailabilities;
                }

            } else {
                Logger.LogError($"Failed to find tour by Id {providerId}"); //throw exception here in real scenario
            }

            return Task.CompletedTask;
        }

    }
}
