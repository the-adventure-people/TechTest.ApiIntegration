using ApiIntegration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.Extensions
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection
                .AddSingleton<IProviderRepository, ProviderRepository>()
                .AddSingleton<ITourRepository, TourRepository>();
        }
    }
}
