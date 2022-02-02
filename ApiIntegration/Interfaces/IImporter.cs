namespace ApiIntegration.Interfaces
{
    using System.Threading.Tasks;

    public interface IImporter
    {
        Task ExecuteAsync(int providerId);
    }
}
