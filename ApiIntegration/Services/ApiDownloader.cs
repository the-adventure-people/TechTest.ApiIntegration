using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ApiIntegration.Services
{
    public class ApiDownloader : IApiDownloader
    {
        private readonly ILogger<ApiDownloader> logger;
        private readonly IApiDownloaderHttpHandler httpHandler;

        public ApiDownloader(ILogger<ApiDownloader> logger,
            IApiDownloaderHttpHandler httpHandler)
        {
            this.logger = logger;
            this.httpHandler = httpHandler;
        }

        public async Task<ApiAvailabilityResponse> Download()
        {
            var httpResponseJson = await httpHandler.GetBodyAsync();

            var response = ConvertHttpResponseToApiAvailability(httpResponseJson);

            return response;
        }

        private ApiAvailabilityResponse ConvertHttpResponseToApiAvailability(string httpResponseJson)
        {
            try
            {
                var apiAvailabilityResponse = JsonConvert.DeserializeObject<ApiAvailabilityResponse>(httpResponseJson);
                return apiAvailabilityResponse;
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "Failed to deserialize json");
                throw;
            }
        }
    }
}
