namespace ApiIntegration.Services
{
    using ApiIntegration.Interfaces;

    public class PricingService : IPricingService
    {
        private readonly decimal _defaultDiscountPercentage = 0.05m;

        public PricingService()
        {
        }

        public PricingService(decimal defaultDiscountPercentage)
        {
            _defaultDiscountPercentage = defaultDiscountPercentage;
        }

        public decimal CalcSellingPrice(decimal providerPrice, decimal commissionPercentage)
        {
            var commission = commissionPercentage * providerPrice;
            var discount = GetDiscountPercentage() * providerPrice;

            var sellingPrice = providerPrice + commission - discount;

            return sellingPrice;
        }

        public virtual decimal GetDiscountPercentage()
        {
            // Note: logic to grab current discount percentages from cache, db, wherever. Hardcoded to default for this technical test.
            return _defaultDiscountPercentage; // 5% discount
        }
    }
}
