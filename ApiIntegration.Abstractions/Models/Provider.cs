namespace ApiIntegration.Models
{
    public class Provider
    {
        public int ProviderId { get; init; }
        public string Name { get; init; }
        public decimal CommissionMultiplier { get; init; }
        public decimal DiscountMultiplier { get; init; }
    }
}
