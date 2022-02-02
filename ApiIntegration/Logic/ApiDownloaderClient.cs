using ApiIntegration.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Logic {
    public class ApiDownloaderClient : IApiDownloaderClient {
        public HttpClient Client { get; }
        public string Uri { get; }

        public ApiDownloaderClient(HttpClient client, string uri) {
            Client = client;
            Uri = uri;
        }

        public async Task<HttpResponseMessage> Download() {
            var response = await Client.GetAsync(Uri).ConfigureAwait(false);
            return response;
        }
    }
}
