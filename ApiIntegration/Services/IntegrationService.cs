using ApiIntegration.Interfaces;
using ApiIntegration.Models.Response;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class IntegrationService : IIntegrationService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string url = "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/";

        public async Task<AvailabilityListResponse> GetProvider()
        {
            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            var AvailabilityList = JsonConvert.DeserializeObject<AvailabilityListResponse>(responseBody);
            return AvailabilityList;
        }
    }
}
