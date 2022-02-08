using ApiIntegration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourRepository
    {
        /// <summary>
        /// Update a tour from the repository.
        /// </summary>
        Task Update(Tour tour);

        /// <summary>
        /// Get a tour from the repository.
        /// </summary>
        Task<Tour> Get(int tourId, string tourRef);

        /// <summary>
        /// Get all tours related to a provider.
        /// </summary>
        Task<List<Tour>> GetByProider(int providerId);

        /// <summary>
        /// Get all tours from the repository.
        /// </summary>
        Task<List<Tour>> GetAll();
    }
}
