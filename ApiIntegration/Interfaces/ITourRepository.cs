using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourRepository
    {
        Task Update(Tour tour);
        Task<Tour> GetAsync(int providerId);
        Task<Tour> GetAsync(string tourRef);
    }
}
