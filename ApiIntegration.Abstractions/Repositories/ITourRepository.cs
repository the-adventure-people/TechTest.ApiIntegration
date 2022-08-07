namespace ApiIntegration.Repositories
{
    using ApiIntegration.Models;

    using System.Threading.Tasks;

    public interface ITourRepository
    {
        Task<bool> UpdateAsync(Tour tour, CancellationToken cancellationToken = default);
        Task<Tour?> GetAsync(int tourId, CancellationToken cancellationToken = default);
        Task<Tour?> GetAsync(string tourRef, CancellationToken cancellationToken = default);
    }
}
