using ApiIntegration.Interfaces;
using ApiIntegration.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class Importer : IImporter
    {
        private readonly ITourRepository tourRepository;
        private readonly IProviderRepository providerRepository;
        private readonly IApiDownloader apiDownloader;
        private readonly ILogger logger;
        private TourService tourService;
        
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
            
            tourService = new TourService(providerRepository); 
        }


        public async Task Execute(int providerId)
        {
            logger.LogInformation("Download Started");

            var providerResponse = await apiDownloader.Download();

            // Transform provider model to our model
            var tourAvailabilities = await tourService.ConvertToTourAvailability(providerResponse);

            // Adjust prices

            tourService.AdjustAllPrices(tourAvailabilities, providerId);
            // Save to repositories

            await tourService.UpdateAvailabilities(tourAvailabilities);

            logger.LogInformation("Download Finished");
        }

        
    }
}
