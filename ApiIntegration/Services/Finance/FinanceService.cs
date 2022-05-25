using ApiIntegration.Data.Repositories;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly IProviderRepository _providerRepository;

        public FinanceService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public async Task<decimal> SetSellingPriceAsync(decimal price, int providerId)
        {
            // TODO: Load discount from a DiscountService, which would look up the relevant Discounts for the given ProviderId. Hard coded for now
            return price
                + await CalculateCommissionAsync(providerId, price)
                - CalculateDiscount(0.05m, price);
        }

        public async Task<decimal> CalculateCommissionAsync(int providerId, decimal price)
        {
            return price * (await _providerRepository.GetAsync(providerId)).Commission;
        }

        public decimal CalculateDiscount(decimal discountToApply, decimal price)
        {
            return price * discountToApply;
        }
    }
}
