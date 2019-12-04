namespace ApiIntegration.Interfaces
{
    public interface IPricingStrategy
    {
        decimal AdjustPrice(decimal price, decimal discount, decimal commission);
    }
}
