namespace ApiIntegration
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.Extensions.Logging;

    public class Importer : IImporter
    {
        private readonly IPriceAdjuster _priceAdjuster;
        private readonly ITransformer _transformer;
        private readonly IApiDownloader apiDownloader;
        private readonly ILogger logger;
        private readonly IProviderRepository providerRepository;
        private readonly ITourRepository tourRepository;

        public Importer(
            ITourRepository tourRepository,
            IProviderRepository providerRepository,
            IApiDownloader apiDownloader,
            ITransformer transformer,
            IPriceAdjuster priceAdjuster,
            ILogger logger)
        {
            this.tourRepository = tourRepository;
            this.providerRepository = providerRepository;
            this.apiDownloader = apiDownloader;
            _transformer = transformer;
            _priceAdjuster = priceAdjuster;
            this.logger = logger;
        }


        public async Task Execute(int providerId)
        {
            logger.LogInformation("Download Started");

            var providerResponse = await apiDownloader.Download();

            // Transform provider model to our model
            var tourAvailabilities = await _transformer.Transform(providerResponse, providerId)
                .ConfigureAwait(false);

            // Adjust prices
            await _priceAdjuster.Adjust(tourAvailabilities.Select(a => a.TourAvailability).ToList(), providerId)
                .ConfigureAwait(false);

            // Save to repositories

            foreach (var availability in tourAvailabilities)
            {
                // necessary to call the repo, needs more work to avoid it (grouping by tour.id)
                var tourToUpdate = await tourRepository.Get(availability.Tour.TourId, availability.Tour.TourRef);
                tourToUpdate.Availabilities.Add(availability.TourAvailability);
                await tourRepository.Update(tourToUpdate)
                    .ConfigureAwait(false);
            }

            logger.LogInformation("Download Finished");
        }

        private async Task<decimal> AdjustPrice(decimal price)
        {
            throw new NotImplementedException();
        }
    }
}