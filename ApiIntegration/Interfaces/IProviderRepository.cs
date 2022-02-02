namespace ApiIntegration.Interfaces
{
    using ApiIntegration.Models;
    using System.Threading.Tasks;

    public interface IProviderRepository
    {
        Task<Provider> Get(int providerId);
    }
}
