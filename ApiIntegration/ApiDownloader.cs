using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;

namespace ApiIntegration
{
    public class ApiDownloader : IApiDownloader
    {
        private static readonly HttpClient Client = new HttpClient();

        public async Task<ApiAvailabilityResponse> Download()
        {
            var serializer = new DataContractJsonSerializer(typeof(ApiAvailabilityResponse));

            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = Client.GetStreamAsync("http://tap.techtest.s3-website.eu-west-2.amazonaws.com/");
            var response = serializer.ReadObject(await streamTask) as ApiAvailabilityResponse;
            return response;
        }
    }
}
