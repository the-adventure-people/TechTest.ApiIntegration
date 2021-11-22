using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ApiIntegration.Services
{
    public class ImporterService : IImporterService
    {
        private readonly ITourRepository tourRepository;
        private readonly IProviderRepository providerRepository;
        private readonly ILogger logger;
        private readonly IIntegrationService integrationService;

        public ImporterService(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IIntegrationService integrationService,
            ILogger logger)
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.integrationService = integrationService;
            this.logger = logger;
        }

        public async Task ExecuteAsync(int providerId)
        {
            logger.LogInformation("Download Started");
            AvailabilityListResponse availabilityListResponse = await integrationService.GetProvider();
            foreach (var availability in availabilityListResponse.AvailabilityList)
                await BuildTourAsync(availability, providerId);
            logger.LogInformation("Download Finished");
        }

        private async Task BuildTourAsync(AvailabilityResponse availabilityResponse, int providerId)
        {
            var tour = await tourRepository.GetAsync(tourRef: availabilityResponse.ProductCode);

            if(tour == null)
            {
                LogNotExists(availabilityResponse);
                return;
            }

            int tourId = tour.TourId;
            availabilityResponse.SetTourId(tourId);
            var provider = await providerRepository.GetAsync(providerId);

            if (provider == null)
            {
                logger.LogInformation($"Object Error provider \nproviderId: {providerId}");
                return;
            }

            availabilityResponse.AdjustPrice(provider.Commission);
            tour.Availabilities.Add(availabilityResponse);
            await tourRepository.UpdateAsync(tour);
        }

        private void LogNotExists(AvailabilityResponse availabilityResponse) 
            => logger.LogInformation($"Object Error: {JsonConvert.SerializeObject(availabilityResponse)}");
    }
}
