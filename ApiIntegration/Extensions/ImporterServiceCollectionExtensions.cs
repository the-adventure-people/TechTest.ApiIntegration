using ApiIntegration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.Extensions
{
    public static class ImporterServiceCollectionExtensions
    {
        public static IServiceCollection AddImporter(this IServiceCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection
                .AddSingleton<IImporter, Importer>();
        }
    }
}
