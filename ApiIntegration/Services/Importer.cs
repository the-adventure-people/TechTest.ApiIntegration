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

            // Download
            var providerResponse = await apiDownloader.Download();

            // Handle downloaded data
            await ImportAvailabilities(new ImportAvailabilitiesRequest
            {
                Availabilities = providerResponse.Body,
                ProviderId = providerId
            });

            logger.LogInformation("Download Finished");
        }


        private async Task ImportAvailabilities(ImportAvailabilitiesRequest req)
        {
            logger.LogInformation("Download complete. Importing to repositories.");

            if (req.Availabilities == null || !req.Availabilities.Any())
                return;
            var provider = await providerRepository.Get(req.ProviderId);

            foreach (var item in req.Availabilities)
            {

            }

            // Transform provider model to our model

            // Adjust prices

            // Save to repositories
        }
    }
}
