using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IApiDownloader
    {
        Task<T> DownloadAsync<T>(string url) where T : class;
    }
}
