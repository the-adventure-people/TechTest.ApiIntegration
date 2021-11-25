using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class ApiDownloader : IApiDownloader
    {
        public async Task<ApiAvailabilityResponse> Download()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            
            var response = await client.GetFromJsonAsync<ApiAvailabilityResponse>("http://tap.techtest.s3-website.eu-west-2.amazonaws.com/");

            return response;
        }
    }
}
