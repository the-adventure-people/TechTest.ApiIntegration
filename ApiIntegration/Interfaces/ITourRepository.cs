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
        Task<Tour> Get(int providerId, string tourRef);

        /// <summary>
        /// Get all tours from the repository.
        /// </summary>
        Task<List<Tour>> GetAll();
    }
}
