using ApiIntegration.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public interface ITourService
    {
        Task<Tour> GetAsync(int tourId = 0);
        Task<Tour> GetAsync(string tourRef = null);
        Task<bool> UpdateAsync(Tour tour);
        Task UpdateToursUsingTourAvailabilities(IEnumerable<TourAvailability> availabilities);
    }
}