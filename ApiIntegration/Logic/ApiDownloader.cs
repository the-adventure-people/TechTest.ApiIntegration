using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Logic {
    public class ApiDownloader : IApiDownloader {
        public IApiDownloaderClient ApiDownloaderClient { get; }
        public ILogger<ApiDownloader> Logger { get; }

        public ApiDownloader(IApiDownloaderClient apiDownloaderClient, ILogger<ApiDownloader> logger) {
            ApiDownloaderClient = apiDownloaderClient;
            Logger = logger;
        }

        public async Task<ApiAvailabilityResponse> Download() {
            var response = await ApiDownloaderClient.Download().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<ApiAvailabilityResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}
