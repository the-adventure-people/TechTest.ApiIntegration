using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Tests
{
    public class TestFactory
    {
        public Mock<ITourRepository> CreateMockTourRepository()
        {
            var mockRepository = new Mock<ITourRepository>();

            return mockRepository;
        }

        public Mock<IProviderRepository> CreateMockProviderRepository()
        {
            var mockRepository = new Mock<IProviderRepository>();

            return mockRepository;
        }

        public Mock<IApiDownloader> CreateMockApiDownloader(List<Availability> availabilities)
        {
            var mockRepository = new Mock<IApiDownloader>();
            mockRepository.Setup(downloader => downloader.DownloadAsync()).Returns(Task.FromResult(CreateApiAvailabilityResponse(availabilities)));

            return mockRepository;
        }

        public IImporter CreateImporter(ITourRepository tourRepository, IProviderRepository providerRepository, IApiDownloader apiDownloader)
        {
            return new Importer(tourRepository,
                providerRepository,
                apiDownloader, 
                new NullLoggerFactory());
        }

        public ApiAvailabilityResponse CreateApiAvailabilityResponse(List<Availability> availabilities)
        {
            return new ApiAvailabilityResponse()
            {
                StatusCode = 200,
                Body = availabilities
            };
        }
    }
}
