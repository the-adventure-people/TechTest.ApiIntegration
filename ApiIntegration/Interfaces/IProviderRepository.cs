using ApiIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiIntegration.Interfaces
{
    public interface IProviderRepository
    {
        Task<Provider> Get(int providerId);

        Task<IEnumerable<Provider>> Get(Expression<Func<Provider, bool>> filter = null, Func<IQueryable<Provider>, IOrderedQueryable<Provider>> orderBy = null);
    }
}
