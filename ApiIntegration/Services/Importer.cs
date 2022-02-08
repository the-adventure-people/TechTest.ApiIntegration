using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class Importer : IImporter
    {
        private readonly ITourRepository tourRepository;
        private readonly IProviderRepository providerRepository;
        private readonly IApiDownloader apiDownloader;
        private readonly ILogger<Importer> logger;

        public Importer(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader,
            ILogger<Importer> logger)
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.apiDownloader = apiDownloader;
            this.logger = logger;
        }


        public async Task Execute(int providerId)
        {
            logger.LogInformation("Download Started");

            // Get provider
            var provider = await GetProvider(providerId);
            if (provider == null)
            {
                logger.LogInformation("Provider not found. Cancelling.");
                return;
            }

            // Download
            var providerResponse = await apiDownloader.Download();

            // Handle downloaded data
            await ImportAvailabilities(new ImportAvailabilitiesRequest
            {
                Availabilities = providerResponse.Body,
                Provider = provider
            });

            logger.LogInformation("Download Finished");
        }

        private async Task<Provider> GetProvider(int providerId)
        {
            return await providerRepository.Get(providerId);
        }

        private async Task ImportAvailabilities(ImportAvailabilitiesRequest req)
        {
            logger.LogInformation("Download complete. Importing to repositories.");

            if (req.Availabilities == null || !req.Availabilities.Any())
                return;

            foreach (var item in req.Availabilities)
            {

            }

            // Transform provider model to our model

            // Adjust prices

            // Save to repositories
        }


    }
}
