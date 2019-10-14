using System.Net.Http;
using System.Threading.Tasks;
using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using Newtonsoft.Json;

namespace ApiIntegration
{
    public class ApiDownloader : IApiDownloader
    {
        private const string ApiUrl = "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/";

        public async Task<ApiAvailabilityResponse> Download()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(ApiUrl);

                response.EnsureSuccessStatusCode();

                var responseAsJsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiAvailabilityResponse>(responseAsJsonString);
            }
        }
    }
}
