namespace ApiIntegration.Repositories
{
    using ApiIntegration.Models;

    using System.Threading.Tasks;

    public interface IProviderRepository
    {
        Task<Provider?> GetAsync(int providerId, CancellationToken cancellationToken = default);
    }
}
