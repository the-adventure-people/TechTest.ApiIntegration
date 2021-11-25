using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class ApiDownloader : IApiDownloader, IDisposable
    {
        private bool _disposed;
        private static readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        static ApiDownloader()
        {
            _httpClient = new HttpClient();
        }

        public ApiDownloader(ILogger logger)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _httpClient.Dispose();
            }

            _disposed = true;
        }

        public async Task<T> DownloadAsync<T>(string url) where T : class
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException($"Argument cannot be null or empty. Parameter name: {nameof(url)}");
            }

            using (var response = await _httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    _logger.LogError($"Download attempt returned with a status code of {response.StatusCode} ({(int)response.StatusCode}).");
                }
            }

            return null;
        }
    }
}