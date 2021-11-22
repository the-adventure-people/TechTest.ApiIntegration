using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourRepository
    {
        Task UpdateAsync(Tour tour);
        Task<Tour> GetAsync(int providerId =default, string tourRef = default);
    }
}
