using System;
using System.Threading.Tasks;
using ApiIntegration.Interfaces;
using ApiIntegration.Logic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ApiIntegration.ConsoleRunner {
    class Program {
        public static ServiceProvider ServiceProvider { get; private set; }


        public static void Setup() {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IApiDownloader, ApiDownloader>();
            serviceCollection.AddSingleton<IApiDownloaderClient>(s => new ApiDownloaderClient(new System.Net.Http.HttpClient(), "http://tap.techtest.s3-website.eu-west-2.amazonaws.com/"));
            serviceCollection.AddSingleton<IImporter, Importer>();
            serviceCollection.AddSingleton<IPriceHandler, SalePriceHandler>();
            serviceCollection.AddSingleton<ITourRepository>(s => new TourRepository());
            serviceCollection.AddSingleton<IProviderRepository, ProviderRepository>();

            serviceCollection.AddLogging(e => e.AddConsole());

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
         static void Main(string[] args) {
            Setup();

            var importer = ServiceProvider.GetService<IImporter>();
            importer.Execute(1).GetAwaiter().GetResult();
        }
    }
}
