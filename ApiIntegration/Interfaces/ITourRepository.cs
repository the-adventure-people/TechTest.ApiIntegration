namespace ApiIntegration.Interfaces
{
    using ApiIntegration.Models;
    using System.Threading.Tasks;

    public interface ITourRepository
    {
        Task Update(Tour tour);
        Task<Tour> Get(int providerId, string tourRef);
    }
}
