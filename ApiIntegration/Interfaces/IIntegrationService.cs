using ApiIntegration.Models.Response;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IIntegrationService
    {
        Task<AvailabilityListResponse> GetProvider();
    }
}
