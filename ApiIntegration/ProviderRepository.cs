using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly Dictionary<int, Provider> providers;


        public ProviderRepository()
        {
            this.providers = new Dictionary<int, Provider>()
            {
                { 1, new Provider()
                    {
                        ProviderId = 1,
                        Name = "Awesome Cycling Holidays",
                        Url = "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/",
                        Commission = 0.15m
                    }
                }
            };
        }

        public Task<Provider> Get(int providerId)
        {
            Provider provider;
            if (!this.providers.TryGetValue(providerId, out provider))
            {
                provider = null;
            }

            return Task.FromResult(provider);
        }
    }
}
