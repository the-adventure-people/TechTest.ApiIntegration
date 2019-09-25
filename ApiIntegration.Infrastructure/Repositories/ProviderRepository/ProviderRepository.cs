using ApiIntegration.Infrastructure.Repositories.Provider;
using ApiIntegration.Models;
using ApiIntegration.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration.Infrastructure.Repositories.ProviderRepository
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly Dictionary<int, ProviderEntity> providers;


        public ProviderRepository()
        {
            providers = new Dictionary<int, ProviderEntity>()
            {
                { 1, new ProviderEntity()
                    {
                        ProviderId = 1,
                        Name = "Awesome Cycling Holidays",
                        Commission = 0.15m
                    }
                }
            };
        }

        public Task<ProviderEntity> Get(int providerId)
        {
            ProviderEntity provider;
            if (!providers.TryGetValue(providerId, out provider))
            {
                provider = null;
            }

            return Task.FromResult(provider);
        }
    }
}
