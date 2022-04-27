using ApiIntegration.ProviderModels;
using System.Threading.Tasks;

namespace ApiIntegration.Services.ApiDownloader
{
    public interface IApiDownloaderService
    {
        Task<ApiAvailabilityResponse> Download();
    }
}
