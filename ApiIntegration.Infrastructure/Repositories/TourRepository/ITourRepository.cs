
using ApiIntegration.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration.Infrastructure.Repositories.Tour
{
    public interface ITourRepository
    {
        Task Update(TourEntity tour);
        Task<TourEntity> Get(int providerId, string tourRef);

        Task<TourEntity> GetByTourRef(string tourRef);

        Task<List<TourEntity>> GetByTourRefs(string[] tourRefs);
    }
}
