using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IImporter
    {
        Task Execute(int providerId);
    }
}
