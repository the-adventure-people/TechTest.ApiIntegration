using ApiIntegration.Models.ResponseModels;

using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourBroker
    {
        Task<ApiAvailabilityResponse> GetTourDataAsync();
    }
}
