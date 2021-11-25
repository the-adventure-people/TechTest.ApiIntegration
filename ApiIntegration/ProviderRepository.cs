using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly Dictionary<int, Provider> providers;


        public ProviderRepository()
        {
            this.providers = new Dictionary<int, Provider>()
            {
                { 1, new Provider()
                    {
                        ProviderId = 1,
                        Name = "Awesome Cycling Holidays",
                        Commission = 0.15m
                    }
                }
            };
        }

        public Task<Provider> Get(int providerId)
        {
            Provider provider;
            if (!this.providers.TryGetValue(providerId, out provider))
            {
                provider = null;
            }

            return Task.FromResult(provider);
        }

        public async Task<IEnumerable<Provider>> Get(Expression<Func<Provider, bool>> filter = null, Func<IQueryable<Provider>, IOrderedQueryable<Provider>> orderBy = null)
        {
            IQueryable<Provider> query = this.providers.Values.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).AsEnumerable();
            }
            else
            {
                return query.AsEnumerable();
            }
        }
    }
}
