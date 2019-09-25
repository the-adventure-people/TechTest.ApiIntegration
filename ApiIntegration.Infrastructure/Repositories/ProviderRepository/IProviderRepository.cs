using ApiIntegration.Models;
using ApiIntegration.Models.Entities;
using System.Threading.Tasks;

namespace ApiIntegration.Infrastructure.Repositories.Provider
{
    public interface IProviderRepository
    {
        Task<ProviderEntity> Get(int providerId);
    }
}
