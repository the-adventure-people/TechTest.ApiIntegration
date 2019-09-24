using System;

namespace ApiIntegrationHarvest
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using ApiIntegration;
    using ApiIntegration.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddHttpClient<IApiDownloader, ApiDownloader>(client =>
                    {
                        client.BaseAddress = new Uri("http://tap.techtest.s3-website.eu-west-2.amazonaws.com/");
                    })
                .SetHandlerLifetime(TimeSpan.FromMinutes(1));

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole();
            });
            
            ILogger logger = loggerFactory.CreateLogger<Program>();

            services.AddSingleton<ILogger>(logger);
            services.AddTransient<IImporter, Importer>();
            services.AddTransient<ITourRepository, TourRepository>();
            services.AddTransient<IProviderRepository, ProviderRepository>();
            services.AddTransient<ITransformer, Transformer>();
            services.AddTransient<IPriceBuilder, PriceBuilder>();
            services.AddTransient<IPriceAdjuster, PriceAdjuster>();

            var serviceProvider = services.BuildServiceProvider();

            var importer = (IImporter) serviceProvider.GetService(typeof(IImporter));

            await importer.Execute(1);
            
            Console.WriteLine("Hello World!");
        }
    }
}