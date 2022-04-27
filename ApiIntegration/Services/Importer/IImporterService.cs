using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IImporterService
    {
        Task Execute(int providerId);
    }
}
