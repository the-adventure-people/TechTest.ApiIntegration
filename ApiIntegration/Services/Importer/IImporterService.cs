using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public interface IImporterService
    {
        Task ExecuteAsync(int providerId);
    }
}
