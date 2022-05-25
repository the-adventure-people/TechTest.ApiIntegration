using ApiIntegration.Data.Models;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public interface IProviderService
    {
        Task<Provider> GetAsync(int providerId);
    }
}