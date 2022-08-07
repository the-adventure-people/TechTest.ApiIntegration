namespace ApiIntegration.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using ApiIntegration.Models;

    public class TourPricingService : ITourPricingService
    {
        public async IAsyncEnumerable<PricedTourAvailability> PriceTourAvailabilityAsync(
            Provider provider, 
            IAsyncEnumerable<TourAvailability> tourAvailabilities, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var tourAvailability in tourAvailabilities)
            {
                decimal sellingPrice = CalculateSellingPrice(provider, tourAvailability);

                yield return new PricedTourAvailability
                {
                    TourId = tourAvailability.TourID,
                    StartDate = tourAvailability.StartDate,
                    TourDuration = tourAvailability.TourDuration,
                    SellingPrice = sellingPrice,
                    AvailabilityCount = tourAvailability.AvailabilityCount
                };
            }
        }

        private static decimal CalculateSellingPrice(Provider provider, TourAvailability tourAvailability)
        {
            decimal cost = tourAvailability.Cost;

            var commission = CalculateTourCommission(cost, provider);
            var discount = CalculateTourDiscount(cost, provider);

            var sellingPrice = cost + commission - discount;
            return sellingPrice;
        }

        private static decimal CalculateTourCommission(decimal cost, Provider provider) =>
            provider.CommissionMultiplier * cost;

        private static decimal CalculateTourDiscount(decimal cost, Provider provider) =>
            provider.DiscountMultiplier * cost;
    }
}
