namespace ApiIntegration.Interfaces
{
    public interface IPricingService
    {
        decimal CalcSellingPrice(decimal commission, decimal price);
    }
}
