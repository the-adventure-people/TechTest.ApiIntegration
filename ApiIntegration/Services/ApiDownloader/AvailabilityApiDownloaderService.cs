using ApiIntegration.Data.Models.External;
using ApiIntegration.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class AvailabilityApiDownloaderService : IApiDownloaderService<ApiAvailabilityResponse>
    {
        private readonly ILogger<IApiDownloaderService<ApiAvailabilityResponse>> _logger;

        private static HttpClient _client;
        private readonly string _endpointUrl;

        public AvailabilityApiDownloaderService(
            ILogger<IApiDownloaderService<ApiAvailabilityResponse>> logger,
            HttpClient client,
            string endpointUrl)
        {
            _logger = logger;
            // TODO: HttpClient should be injected here
            _client = new HttpClient();
            // TODO: Load URL from ProviderService
            _endpointUrl = "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/";
        }

        public async Task<ApiAvailabilityResponse> DownloadAsync()
        {
            string responseString = string.Empty;
            ApiAvailabilityResponse availabilityResponse = null;

            var startTime = DateTime.Now;

            try
            {
                var response = await _client.GetAsync(_endpointUrl);

                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync();

                    availabilityResponse = JsonConvert.DeserializeObject<ApiAvailabilityResponse>(responseString);
                }
                else
                {
                    _logger.LogError(Events.ApiDownloaderService, "Failure HTTP status code received from server.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(Events.ApiDownloaderService, ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation(
                    Events.ApiDownloaderService, 
                    "{0}{1}{2}",
                    Helpers.Duration(startTime),
                    Helpers.FormatLogInfo(Helpers.RequestTitle, _endpointUrl),
                    Helpers.FormatLogInfo(Helpers.ResponseTitle, responseString));
            }

            return availabilityResponse;
        }
    }
}
