namespace ApiIntegration.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using ProviderModels;

    public interface ITransformer
    {
        Task<ICollection<TourAvailabilityWithTour>> Transform(ApiAvailabilityResponse apiAvailabilityResponse,
            int providerId);
    }
}