namespace ApiIntegration.Providers
{
    using System.Threading.Tasks;

    public interface IAvailabilityApi<TProviderResponse>
    {
        Task<TProviderResponse?> GetAvailabilityDataAsync(CancellationToken cancellationToken = default);
    }
}
