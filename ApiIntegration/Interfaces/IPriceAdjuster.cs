namespace ApiIntegration.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IPriceAdjuster
    {
        Task<ICollection<TourAvailability>> Adjust(ICollection<TourAvailability> tourAvailabilities, int providerId);
    }
}