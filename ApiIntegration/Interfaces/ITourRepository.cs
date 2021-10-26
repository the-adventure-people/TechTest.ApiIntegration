using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourRepository
    {
        Task UpdateTourAsync(Tour tour);

        Task UpdateTourAvailabilityAsync(TourAvailability tourAvailability);

        Task<Tour> GetTourAsync(int providerId, string tourRef);
    }
}
