namespace ApiIntegration.Services
{
    using ApiIntegration.Interfaces;
    using ApiIntegration.ProviderModels;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class ApiDownloader : IApiDownloader
    {
        private readonly IHttpClientFactory _clientFactory;

        public ApiDownloader(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ApiAvailabilityResponse> Download()
        {
            var client = _clientFactory.CreateClient("ProviderClient");

            // Uses base url defined in startup
            var request = new HttpRequestMessage(HttpMethod.Get, "/");

            // Automatic retries according to Polly policy
            var response = await client.SendAsync(request);

            // This makes sure an exception is thrown if call is not a success
            response.EnsureSuccessStatusCode();

            var responseAvailability = await response.Content.ReadFromJsonAsync<ApiAvailabilityResponse>();

            return responseAvailability;
        }
    }
}
