namespace ApiIntegration.Import
{
    using System.Threading.Tasks;

    public interface IImporter<TProviderResponse>
    {
        Task ExecuteAsync(int providerId, CancellationToken cancellationToken = default);
    }
}
