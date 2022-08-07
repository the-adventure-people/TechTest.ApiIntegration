namespace ApiIntegration.Repositories
{
    using ApiIntegration.Models;

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

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
                        CommissionMultiplier = 0.15m,
                        DiscountMultiplier = 0.05m
                    }
                }
            };
        }

        public Task<Provider?> GetAsync(int providerId, CancellationToken cancellationToken = default) => 
            Task.FromResult(_providers.TryGetValue(providerId, out var provider)
                ? provider
                : null);
    }
}
