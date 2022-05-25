using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public interface IFinanceService
    {
        Task<decimal> SetSellingPriceAsync(decimal price, int providerId);
        Task<decimal> CalculateCommissionAsync(int providerId, decimal price);
        decimal CalculateDiscount(decimal discountToApply, decimal price);
    }
}
