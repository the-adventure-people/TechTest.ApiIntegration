using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourRepository
    {
        Task Update(Tour tour);
        Task<Tour> Get(int providerId, string tourRef);
    }
}
