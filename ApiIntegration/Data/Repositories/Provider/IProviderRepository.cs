using ApiIntegration.Data.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Data.Repositories
{
    public interface IProviderRepository
    {
        Task<Provider> GetAsync(int providerId);
    }
}
