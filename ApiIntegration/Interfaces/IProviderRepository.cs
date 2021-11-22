using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IProviderRepository
    {
        Task<Provider> GetAsync(int providerId);
    }
}
