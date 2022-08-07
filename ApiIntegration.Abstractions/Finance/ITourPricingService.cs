namespace ApiIntegration.Finance
{
    using ApiIntegration.Models;

    public interface ITourPricingService
    {
        IAsyncEnumerable<PricedTourAvailability> PriceTourAvailabilityAsync(
            Provider provider,
            IAsyncEnumerable<TourAvailability> tourAvailabilities,
            CancellationToken cancellationToken = default);
    }
}
