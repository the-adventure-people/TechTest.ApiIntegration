using System.Collections.Generic;
using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourRepository
    {
        Task Update(Tour tour);
        Task<Tour> Get(int tourId, string tourRef);
        Task<Tour> Get(int tourId);
        Task<Dictionary<int, Tour>> GetAllTours();

    }
}
