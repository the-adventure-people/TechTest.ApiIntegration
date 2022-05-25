using ApiIntegration.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration.Data.Repositories
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

        public Task<Provider> GetAsync(int providerId)
        {
            Provider provider;
            if (!this._providers.TryGetValue(providerId, out provider))
            {
                provider = null;
            }

            return Task.FromResult(provider);
        }
    }
}
