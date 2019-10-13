using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly Dictionary<int, Provider> _providers;

        public ProviderRepository()
        {
            _providers = new Dictionary<int, Provider>()
            {
                { 1, new Provider()
                    {
                        ProviderId = 1,
                        Name = "Awesome Cycling Holidays",
                        Commission = 0.15m
                    }
                }
            };
        }

        public Task<Provider> Get(int providerId)
        {
            _providers.TryGetValue(providerId, out var provider);
            return Task.FromResult(provider);
        }
    }
}
