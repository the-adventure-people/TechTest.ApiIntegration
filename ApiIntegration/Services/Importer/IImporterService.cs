using System.Threading.Tasks;

namespace ApiIntegration.Core.Services.Importer
{
    public interface IImporterService
    {
        Task Execute(int providerId);
    }
}
