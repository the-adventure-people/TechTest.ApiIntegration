using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration.Data
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly Dictionary<int, Provider> providers;


        public ProviderRepository()
        {
            providers = new Dictionary<int, Provider>()
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
            Provider provider;
            if (!providers.TryGetValue(providerId, out provider))
            {
                provider = null;
            }

            return Task.FromResult(provider);
        }
    }
}
