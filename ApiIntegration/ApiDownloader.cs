namespace ApiIntegration
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using ProviderModels;

    public class ApiDownloader : IApiDownloader
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public ApiDownloader(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<ApiAvailabilityResponse> Download()
        {
            var response = await _httpClient.GetAsync(string.Empty)
                .ConfigureAwait(false);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<ApiAvailabilityResponse>();
                }

                _logger.Log(LogLevel.Error, 1, $"Download was unsuccessful. {response.StatusCode}");
                throw new InvalidOperationException("Failed download");
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, 1, e, e.Message); 
                throw;
            }
        }
    }
}