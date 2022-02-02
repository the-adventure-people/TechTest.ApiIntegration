namespace ApiIntegration.ConsoleApp
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ApiIntegration.Interfaces;
    using ApiIntegration.Models;
    using ApiIntegration.Repos;
    using ApiIntegration.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Polly;
    using Polly.Extensions.Http;

    public class Program
    {
        private static IConfiguration _config;

        static async Task Main(string[] args)
        {
            // Grab settings from appsetttnigs.json
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile($"appsettings.json", true, true)
                .Build();


            // Register services
            var services = new ServiceCollection();
            ConfigureServices(services);
            var servieProvider = services.BuildServiceProvider();

            // Run import process
            var importer = servieProvider.GetService<IImporter>();

            do
            {
                await importer.ExecuteAsync(1);
                Console.WriteLine("Press 'q' to quit, any other key to re-run...\r\n");

            } while (Console.ReadKey().Key != ConsoleKey.Q);

        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(ApiConsoleLogger.Create<Program>());
            services.AddSingleton<ITourRepository, TourRepository>();
            services.AddSingleton<IProviderRepository, ProviderRepository>();
            services.AddSingleton<IImporter,Importer>();
            services.AddSingleton<IApiDownloader, ApiDownloader>();
            services.AddSingleton<IPricingService, PricingService>();


            // Setup HttpClient for HttpClientFactory
            var tourProviderConfig = _config.GetSection(TourProviderOptions.SectionName).Get<TourProviderOptions>();
            services
                .AddHttpClient("ProviderClient", client => client.BaseAddress = new Uri(tourProviderConfig.BaseUrl))
                .AddPolicyHandler(GetRetryPolicy());
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}