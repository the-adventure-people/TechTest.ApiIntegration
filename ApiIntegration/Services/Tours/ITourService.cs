using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Services.Tours
{
    public interface ITourService
    {
        Task<Tour> GetAsync(int tourId, string tourRef = null);
        Task UpdateAsync(Tour tour);
    }
}