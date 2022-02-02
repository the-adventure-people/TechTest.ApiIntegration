using ApiIntegration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourRepository
    {
        Task Update(Tour tour);
        Task<Tour> GetByTourRef(string tourRef);
        Task<Tour> GetByTourId(int tourId);
        Task AddTourAvailability(TourAvailability tourAvailability, string tourRef);
        Task<List<Tour>> GetAll();
    }
}
