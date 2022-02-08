using ApiIntegration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class ApiDownloaderHttpHandler : IApiDownloaderHttpHandler
    {
        #region Static values
        private static readonly string configurationAvailabilityApiEndpoint = "AvailabilityApiEndpoint";
        #endregion

        private static readonly HttpClient httpClient;
        private readonly ILogger<ApiDownloaderHttpHandler> logger;
        private readonly IConfiguration configuration;

        static ApiDownloaderHttpHandler()
        {
            httpClient = new HttpClient();
        }

        public ApiDownloaderHttpHandler(ILogger<ApiDownloaderHttpHandler> logger,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<string> GetBodyAsync()
        {
            HttpResponseMessage httpReply;

            try
            {
                httpReply = await httpClient.GetAsync(configuration[configurationAvailabilityApiEndpoint]);
                httpReply.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "There was a problem making the HTTP request.");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error.");
                throw;
            }

            var httpResponseJson = await httpReply.Content.ReadAsStringAsync();

            return httpResponseJson;
        }
    }
}
