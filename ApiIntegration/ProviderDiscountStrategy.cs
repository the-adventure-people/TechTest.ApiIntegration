using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace ApiIntegration
{
    public class ProviderDiscountStrategy : IPricingStrategy
    {
        private readonly ILogger<ProviderDiscountStrategy> logger;

        public ProviderDiscountStrategy(ILogger<ProviderDiscountStrategy> logger)
        {
            this.logger = logger;
        }

        public decimal AdjustPrice(decimal price, decimal percentageDiscount, decimal commission)
        {
            if (percentageDiscount > 1 || percentageDiscount < 0)
            {
                var errorMessage = $"A percentage discount of '{percentageDiscount}' is invalid, it must fall between the range [0.0 - 1.0]";
                logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            if (commission < 0)
            {
                var errorMessage = $"A commission of '{commission}' is invalid, it cannot be a negative value";
                logger.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            return (price * (1m - percentageDiscount)) + (price * commission);
        }
    }
}
