using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IProviderRepository
    {
        /// <summary>
        /// Get a provider from the repository.
        /// </summary>
        Task<Provider> Get(int providerId);
    }
}
