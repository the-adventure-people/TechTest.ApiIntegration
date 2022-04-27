using ApiIntegration.ProviderModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiIntegration.Services.ApiDownloader
{
    public class ApiDownloaderService : IApiDownloaderService
    {
        private readonly HttpClient _apiClient;
        private readonly string _baseAddress = "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/";

        public ApiDownloaderService()
        {
            // Poor mans IoC, Ideally the APIDownloaderService should inject in a wrapper class around httpClient specifically for the providers API
            // Its not the job of the APIDownloaderService to manage the connection to the API either, should stick to SRP
            _apiClient = new HttpClient();
        }

        public async Task<ApiAvailabilityResponse> Download()
        {
            var response = await GetResponseAsync<ApiAvailabilityResponse>();
            return response;
        }

        public async Task<IResponse> GetResponseAsync<IResponse>()
        {
            var stringResponse = await ReadStringResponseFromUrlAsync(_baseAddress);
            return JsonConvert.DeserializeObject<IResponse>(stringResponse);
        }

        private async Task<string> ReadStringResponseFromUrlAsync(string endpointRoute)
        {
            endpointRoute = endpointRoute ?? string.Empty;

            using (HttpResponseMessage response = await _apiClient.GetAsync(endpointRoute))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
