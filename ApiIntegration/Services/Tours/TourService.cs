using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ApiIntegration.Services.Tours
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly ILogger _logger;

        public TourService(ITourRepository tourRepository, ILogger logger)
        {
            _tourRepository = tourRepository;
            _logger = logger;
        }

        public Task<Tour> GetAsync(int tourId, string tourRef = null)
        {
            _logger.LogInformation("Retreiving Tour");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return _tourRepository.GetAsync(tourId, tourRef);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong while retrieving tour with id {0}", tourId);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Tour retrevied in {0}ms", stopWatch.ElapsedMilliseconds);
            }
        }

        public Task UpdateAsync(Tour tour)
        {
            _logger.LogInformation("Updating Tour");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return _tourRepository.UpdateAsync(tour);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong while updating tour with id {0}", tour.TourId);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Tour updated in {0}ms", stopWatch.ElapsedMilliseconds);
            }
        }
    }
}
