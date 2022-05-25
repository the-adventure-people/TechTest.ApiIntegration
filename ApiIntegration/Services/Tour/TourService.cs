using ApiIntegration.Data.Models;
using ApiIntegration.Data.Repositories;
using ApiIntegration.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;

        private readonly ILogger<ITourService> _logger;

        public TourService(ITourRepository tourRepository, ILogger<ITourService> logger)
        {
            _tourRepository = tourRepository;
            _logger = logger;
        }

        public async Task<Tour> GetAsync(int tourId)
        {
            Tour tour = null;

            var startTime = DateTime.Now;

            try
            {
                tour = await _tourRepository.GetAsync(tourId);
            }
            catch (Exception ex)
            {
                _logger.LogError(Events.TourService, ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation(Events.TourService, Helpers.Duration(startTime));
            }

            return tour;
        }

        public async Task<Tour> GetAsync(string tourRef)
        {
            Tour tour = null;

            var startTime = DateTime.Now;

            try
            {
                tour = await _tourRepository.GetAsync(tourRef);
            }
            catch (Exception ex)
            {
                _logger.LogError(Events.TourService, ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation(Events.TourService, Helpers.Duration(startTime));
            }

            return tour;
        }

        public async Task<bool> UpdateAsync(Tour tour)
        {
            var success = false;

            var startTime = DateTime.Now;

            try
            {
                success = await _tourRepository.UpdateAsync(tour);
            }
            catch (Exception ex)
            {
                _logger.LogError(Events.TourService, ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation(Events.TourService, Helpers.Duration(startTime));
            }

            return success;
        }

        public async Task UpdateToursUsingTourAvailabilities(IEnumerable<TourAvailability> availabilities)
        {
            var startTime = DateTime.Now;

            try
            {
                await Task.WhenAll(availabilities.Select(async availability => {

                    var tour = await GetAsync(availability.TourId);

                    if (!(tour is null))
                    {
                        tour.Availabilities.Add(availability);
                        await UpdateAsync(tour);
                    }
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(Events.ImporterService, ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation(Events.ImporterService, "{0}", Helpers.Duration(startTime));
            }
        }
    }
}
