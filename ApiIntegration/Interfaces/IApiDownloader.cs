namespace ApiIntegration.Interfaces
{
    using ApiIntegration.ProviderModels;
    using System.Threading.Tasks;

    public interface IApiDownloader
    {
        Task<ApiAvailabilityResponse> Download();
    }
}
