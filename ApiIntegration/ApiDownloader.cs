using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using Newtonsoft.Json;

namespace ApiIntegration
{
    public class ApiDownloader : IApiDownloader
    {
        private const string ApiUrl = "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/";

        public async Task<ApiAvailabilityResponse> Download()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(ApiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiAvailabilityResponse { StatusCode = (int)response.StatusCode };
                }

                var responseContentStr = await response.Content.ReadAsStringAsync();
                var tourAvailability = JsonConvert.DeserializeObject<List<Availability>>(responseContentStr);

                return new ApiAvailabilityResponse { Body = tourAvailability, StatusCode = (int)response.StatusCode };
            }
        }
    }
}
