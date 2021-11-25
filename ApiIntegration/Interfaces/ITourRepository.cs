using ApiIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface ITourRepository
    {
        Task Update(Tour tour);
        Task<Tour> Get(int providerId, string tourRef);

        /// <summary>
        /// Get Function that will query the available tours according to any input filter. Can also be ordered.
        /// </summary>
        /// <param name="filter">filter to apply to the whole collection of tours</param>
        /// <param name="orderBy">function for applying ordering to the returned collection</param>
        /// <returns></returns>
        Task<IEnumerable<Tour>> Get(Expression<Func<Tour, bool>> filter = null, Func<IQueryable<Tour>, IOrderedQueryable<Tour>> orderBy = null);
    }
}
