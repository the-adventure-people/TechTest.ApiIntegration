namespace ApiIntegration
{
    using Interfaces;
    using Models;

    public class PriceBuilder : IPriceBuilder
    {
        public decimal Build(Provider provider, decimal price)
        {
            var adjustedPrice = price + price * provider.Commission;
            adjustedPrice -= price * 0.05m;
            return adjustedPrice;
        }
    }
}