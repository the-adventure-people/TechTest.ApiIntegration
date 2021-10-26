using ApiIntegration.Helpers;
using ApiIntegration.Interfaces;
using ApiIntegration.Services;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration.Extensions
{
    public static class ApiDownloaderServiceCollectionExtensions
    {
        public static IServiceCollection AddApiDownloader(this IServiceCollection collection, string baseUrl)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));

            RestClient restClient = new RestClient(baseUrl);
            restClient.AddHandler("application/json", () => { return new NewtonsoftJsonRestSerializer(); });
            restClient.UseNewtonsoftJson();

            return collection
                .AddSingleton<IRestClient>(restClient)
                .AddSingleton<IApiDownloader, ApiDownloader>();
        }
    }
}
