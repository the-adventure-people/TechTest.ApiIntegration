using ApiIntegration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourRepository
    {
        Task Update(Tour tour);
        Task<Tour> Get(int providerId, string tourRef);
        Task UpdateTourAvailability(int providerId, int tourId, List<TourAvailability> tourAvailabilities);
    }
}
