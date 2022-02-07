using ApiIntegration.ProviderModels;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IApiDownloader
    {
        /// <summary>
        /// Download tour availability from the API.
        /// </summary>
        Task<ApiAvailabilityResponse> Download();
    }
}
