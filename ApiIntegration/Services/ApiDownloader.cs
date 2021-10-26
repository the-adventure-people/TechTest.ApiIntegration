using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class ApiDownloader : IApiDownloader
    {
        private readonly IRestClient _restClient;

        public ApiDownloader(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<ApiAvailabilityResponse> DownloadAsync()
        {
            RestRequest request = new RestRequest("", Method.GET);
            var response = await _restClient.ExecuteAsync<ApiAvailabilityResponse>(request);
            
            return response.Data;
        }
    }
}
