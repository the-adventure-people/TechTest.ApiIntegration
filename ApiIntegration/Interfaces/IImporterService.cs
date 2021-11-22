using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IImporterService
    {
        Task ExecuteAsync(int providerId);
    }
}
