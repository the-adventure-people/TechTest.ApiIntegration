using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IApiDownloaderHttpHandler
    {
        /// <summary>
        /// Get the body of the HTTP request to the API endpoint.
        /// </summary>
        Task<string> GetBodyAsync();
    }
}
