using ApiIntegration.ProviderModels;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IApiDownloader
    {
        Task<ApiAvailabilityResponse> Download(string url);
    }
}
