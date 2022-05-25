using ApiIntegration.Data.Models;
using ApiIntegration.Data.Repositories;
using ApiIntegration.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly ILogger<IProviderService> _logger;

        public ProviderService(IProviderRepository providerRepository, ILogger<IProviderService> logger)
        {
            _providerRepository = providerRepository;
            _logger = logger;
        }

        public async Task<Provider> GetAsync(int providerId)
        {
            Provider provider = null;

            var startTime = DateTime.Now;

            try
            {
                provider = await _providerRepository.GetAsync(providerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(Events.ProviderService, ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation(Events.ProviderService, Helpers.Duration(startTime));
            }

            return provider;
        }
    }
}
