namespace ApiIntegration.Providers
{
    using ApiIntegration.Models;

    public interface IProviderResponseAdapter<TProviderResponse>
    {
        IAsyncEnumerable<TourAvailability> ConvertToTourAvailabilityAsync(
            TProviderResponse response,
            CancellationToken cancellationToken = default);
    }
}
