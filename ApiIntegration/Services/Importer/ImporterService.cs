using ApiIntegration.Core.Extensions;
using ApiIntegration.Infrastructure.Repositories.Provider;
using ApiIntegration.Infrastructure.Repositories.Tour;
using ApiIntegration.Interfaces;
using ApiIntegration.Models.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration.Core.Services.Importer
{
    public class ImporterService : IImporterService
    {
        private readonly ITourRepository tourRepository;
        private readonly IProviderRepository providerRepository;
        private readonly ITourBroker tourBroker;
        private readonly ILogger logger;

        public ImporterService(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            ITourBroker tourBroker,
            ILogger logger)
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.tourBroker = tourBroker;
            this.logger = logger;
        }

        //Consider splitting this out - there is quite a lot happening over here.
        public async Task Execute(int providerId)
        {
            //Consider logging some benchmarks over here for dashboards in elastic - we wanna monitor how long it takes to run off how long this process takes.
            logger.LogInformation("Download Started");
            try
            {
                var providerResponse = await tourBroker.GetTourDataAsync();

                var tourAvailabilities = TourAvailabilityEntity.ToTourAvailabilityEntity(providerResponse.Body, 1);
                //get our product refs and match them to our tours.
                var productCodes = providerResponse.Body.Select(avail => avail.ProductCode).ToArray();

                //now lets get all the tours that are relevant to us - performance do one call to the database for everything .
                var tours = await tourRepository.GetByTourRefs(productCodes);
                var provider = await providerRepository.Get(providerId);

                foreach (var tour in tours)
                {
                    var relevantAvailabilities = tourAvailabilities.Where(avail => avail.ProductCode == tour.TourRef).ToList();

                    foreach (var tourAvailability in relevantAvailabilities)
                    {
                        tourAvailability.AdultPrice = tourAvailability.AdultPrice.AdjustPrice(provider.Commission);
                    };

                    tour.Availabilities.AddRange(relevantAvailabilities);
                    //I suppose this is ok - one could consider pushing the availibilities into the array and let the db handle that.
                    await tourRepository.Update(tour);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error occured while attempting to import tour availibility.");
                throw;
            }

            logger.LogInformation("Download Finished");
        }

    }
}
