using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IProviderRepository
    {
        Task<Provider> Get(int providerId);
    }
}
