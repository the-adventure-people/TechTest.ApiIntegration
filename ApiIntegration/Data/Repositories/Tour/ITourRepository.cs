using ApiIntegration.Data.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Data.Repositories
{
    public interface ITourRepository
    {
        Task<bool> UpdateAsync(Tour tour);
        Task<Tour> GetAsync(int tourId);
        Task<Tour> GetAsync(string tourRef);
    }
}
