using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourRepository
    {
        Task Update(Tour tour);

        //Fix: providerId changed to tourId
        Task<Tour> Get(int tourId, string tourRef);

        //More typical methods for getting Tour.
        Task<Tour> Get(int tourId);
        Task<Tour> FindByTourRef(string tourRef);
    }
}
