namespace ApiIntegration.Providers.AwesomeCyclingHolidays
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using ApiIntegration.Providers;
    using ApiIntegration.Providers.AwesomeCyclingHolidays.Models;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class AvailabilityApi : IAvailabilityApi<AvailabilityResponse>
    {
        private readonly ILogger<AvailabilityApi> _logger;
        private readonly Settings _settings;
        private readonly HttpClient _httpClient;

        public AvailabilityApi(
            ILogger<AvailabilityApi> logger,
            IOptions<Settings> options,
            HttpClient httpClient)
        {
            _logger = logger;
            _settings = options.Value;
            _httpClient = httpClient;
        }

        public async Task<AvailabilityResponse?> GetAvailabilityDataAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                var url = _settings.URL;
                _logger.LogInformation("Request: {requestUrl}", url);
                
                var response = await _httpClient.GetAsync(url, cancellationToken);
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogInformation("Response: {responseBody}", responseBody);

                response.EnsureSuccessStatusCode();

                return JsonSerializer.Deserialize<AvailabilityResponse>(responseBody, 
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred getting provider availability data.");
                return null;
            }
        }
    }
}
