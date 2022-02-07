using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IImporter
    {
        /// <summary>
        /// Initiate an import. Supply the provider ID that must be imported.
        /// </summary>
        Task Execute(int providerId);
    }
}
