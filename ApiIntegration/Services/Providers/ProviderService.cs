using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ApiIntegration.Services.Providers
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly ILogger _logger;

        public ProviderService(IProviderRepository providerRepository, ILogger logger)
        {
            _providerRepository = providerRepository;
            _logger = logger;
        }

        public Task<Provider> Get(int providerId)
        {
            _logger.LogInformation("Retreiving Provider");
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return _providerRepository.GetAsync(providerId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong while retrieving provider with id {0}", providerId);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("Provider retrevied in {0}ms", stopWatch.ElapsedMilliseconds);
            }
        }
    }
}
