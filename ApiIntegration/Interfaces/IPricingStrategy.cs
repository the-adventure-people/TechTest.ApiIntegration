using ApiIntegration.Models;

namespace ApiIntegration.Interfaces
{
    public interface IPricingStrategy
    {
        decimal CalculatePrice(Provider provider, decimal originalPrice);
    }
}
