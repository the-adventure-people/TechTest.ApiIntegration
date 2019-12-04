using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using ApiIntegration.Models;
using System.Collections.Generic;
using ApiIntegration.ProviderModels;

namespace ApiIntegration
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

            var providerInfo = await providerRepository.Get(providerId);
            if (providerInfo == null)
            {
                var errorMessage = $"A provider with Id '{providerId}' could not be found";
                logger.LogError(errorMessage);
                throw new KeyNotFoundException(errorMessage);
            }

            var providerResponse = await apiDownloader.Download(providerInfo.Url);

            // Transform provider model to our model

            // Adjust prices

            // Save to repositories

            logger.LogInformation("Download Finished");
        }

        private async Task<decimal> AdjustPrice(decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
