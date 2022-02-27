using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class ApiDownloader : IApiDownloader
    {
        private HttpClient _httpClient;
        private string _url = "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/"; 

        public ApiDownloader()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_url)                
            };
        }

        public async Task<ApiAvailabilityResponse> Download()
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, "/");
            var response = await _httpClient.SendAsync(httpRequest);
            ApiAvailabilityResponse availabilityResponse = new ApiAvailabilityResponse();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                availabilityResponse = JsonConvert.DeserializeObject<ApiAvailabilityResponse>(await response.Content.ReadAsStringAsync());
            }
            
            return availabilityResponse;
        }
    }
}
