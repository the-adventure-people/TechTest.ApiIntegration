using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using Microsoft.Extensions.Logging;
using System;

namespace ApiIntegration
{
    // Note: I've extended the provider entity with a discount value for the following reasons:
    //     1. I'm unsure if this is a global or provider specific discount
    //     2. It means I do not need to add a form of global setting (config file, or additional entity type) but still demonstrate how this feature would work
    //     3. I feel the provider entity could be extended with further properties to affect the pricing which is why I've used the provider entity as a parameter
    public class DiscountedPricingStrategy : IPricingStrategy
    {
        private readonly ILogger _logger;

        public DiscountedPricingStrategy(ILogger logger)
        {
            _logger = logger;
        }

        public decimal CalculatePrice(Provider provider, decimal originalPrice)
        {
            if (provider.Discount < 0 || provider.Discount > 1)
            {
                throw new ArgumentException($"Discount value must be a value between 0 and 1.");
            }

            if (provider.Commission < 0)
            {
                throw new ArgumentException($"Commission value must be a value greater than 0.");
            }

            if (originalPrice < 0)
            {
                throw new ArgumentException($"Provider price value must be a value greater than 0.");
            }

            // It may be worth considering a hardstop to ensure no prices are lower than a set value (original price or zero)

            return originalPrice * (1 + provider.Commission - provider.Discount);
        }
    }
}