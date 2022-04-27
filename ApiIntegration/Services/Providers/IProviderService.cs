using ApiIntegration.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Services.Providers
{
    public interface IProviderService
    {
        Task<Provider> Get(int providerId);
    }
}