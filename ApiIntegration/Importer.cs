﻿using ApiIntegration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class Importer : IImporter
    {
        private readonly ITourRepository tourRepository;
        private readonly IProviderRepository providerRepository;
        private readonly IApiDownloader apiDownloader;
        private readonly ILogger logger;
        private readonly ModelConverter converter;
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
            converter = new ModelConverter();
        }


        public async Task Execute(int providerId)
        {
            logger.LogInformation("Download Started");

            var providerResponse = await apiDownloader.Download();

            // Transform provider model to our model
            var tourAvailabilities = converter.ConvertToTourAvailability(providerResponse);

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
