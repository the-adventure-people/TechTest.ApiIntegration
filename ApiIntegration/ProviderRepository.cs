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
                        ApiEndpoint = "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/",
                        Commission = 0.15m,
                        Discount = 0.05m
                    }
                }
            };
        }

        public Task<Provider> GetAsync(int providerId)
        {
            Provider provider;
            if (!_providers.TryGetValue(providerId, out provider))
            {
                provider = null;
            }

            return Task.FromResult(provider);
        }
    }
}
