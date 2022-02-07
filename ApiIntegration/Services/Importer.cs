using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class Importer : IImporter
    {
        private readonly ITourRepository tourRepository;
        private readonly IProviderRepository providerRepository;
        private readonly IApiDownloader apiDownloader;
        private readonly ILogger logger;

        public Importer(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader, 
            ILogger logger)
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.apiDownloader = apiDownloader;
            this.logger = logger;
        }


        public async Task Execute(int providerId)
        {
            logger.LogInformation("Download Started");

            var providerResponse = await apiDownloader.Download();

            // Transform provider model to our model

            // Adjust prices

            // Save to repositories

            logger.LogInformation("Download Finished");
        }
    }
}
