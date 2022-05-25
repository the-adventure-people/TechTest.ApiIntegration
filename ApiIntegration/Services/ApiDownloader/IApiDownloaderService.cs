using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public interface IApiDownloaderService<TResponse>
    {
        Task<TResponse> DownloadAsync();
    }
}
