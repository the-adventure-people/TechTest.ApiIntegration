using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class ApiDownloader : IApiDownloader
    {
        private static readonly HttpClient httpClient;
        private readonly ILogger<ApiDownloader> logger;

        static ApiDownloader()
        {
            // Only one instance of HttpClient is instantiated to prevent the overuse of sockets.
            // See: https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netframework-4.8#remarks
            httpClient = new HttpClient();
        }

        public ApiDownloader(ILogger<ApiDownloader> logger)
        {
            this.logger = logger;
        }

        public async Task<ApiAvailabilityResponse> Download(string url)
        {
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiAvailabilityResponse = JsonConvert.DeserializeObject<ApiAvailabilityResponse>(content);

                if (apiAvailabilityResponse.StatusCode != (int)HttpStatusCode.OK)
                {
                    throw new HttpRequestException($"Request to provider failed with error code '{apiAvailabilityResponse.StatusCode}'");
                }

                return apiAvailabilityResponse;
            } catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to download availabilies from {url}");
                throw;
            }
        }
    }
}
