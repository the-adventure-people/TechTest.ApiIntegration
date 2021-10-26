using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IImporter
    {
        Task ExecuteAsync(int providerId);
    }
}
