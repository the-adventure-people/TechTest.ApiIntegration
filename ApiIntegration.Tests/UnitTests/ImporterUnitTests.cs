using ApiIntegration.Interfaces;
using ApiIntegration.ProviderModels;
using ApiIntegration.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntegration.Tests.UnitTests
{
    public class ImporterUnitTests
    {
        [Test]
        public async Task Importer_AddNewAvailability_NewAvailabilityShouldBeAdded()
        {
            // Arrange
            TestFactory testFactory = new TestFactory();

            List<Availability> availabilities = new List<Availability>()
                {
                    new Availability()
                    {
                        ProductCode = "EUR123",
                        DepartureDate = "2020-06-20",
                        Nights = 5,
                        Price = 500.0m,
                        Spaces = 8
                    }
                };

            TourRepository tourRepository = new TourRepository();
            ProviderRepository providerRepository = new ProviderRepository();

            IImporter importer = testFactory.CreateImporter(
                tourRepository,
                providerRepository,
                testFactory.CreateMockApiDownloader(availabilities).Object
                );

            // Act
            var tour = await tourRepository.GetTourAsync(1, "EUR123");
            Assert.AreEqual(0, tour.Availabilities.Where(x => x.TourDuration == 5 && x.StartDate == DateTime.Parse("2020-06-20")).Count());
            await importer.ExecuteAsync(1);
            tour = await tourRepository.GetTourAsync(1, "EUR123");

            //Assert
            Assert.AreEqual(1, tour.Availabilities.Where(x => x.TourDuration == 5 && x.StartDate == DateTime.Parse("2020-06-20")).Count());
        }

        [Test]
        public async Task Importer_AddExistingAvailability_ExistingAvailabilityShouldNotBeUpdated()
        {
            // Arrange
            TestFactory testFactory = new TestFactory();

            List<Availability> availabilities = new List<Availability>()
                {
                    new Availability()
                    {
                        ProductCode = "EUR123",
                        DepartureDate = "2020-06-20",
                        Nights = 6,
                        Price = 250.0m,
                        Spaces = 2
                    }
                };

            TourRepository tourRepository = new TourRepository();
            ProviderRepository providerRepository = new ProviderRepository();

            IImporter importer = testFactory.CreateImporter(
                tourRepository,
                providerRepository,
                testFactory.CreateMockApiDownloader(availabilities).Object
                );

            // Act
            var tour = await tourRepository.GetTourAsync(1, "EUR123");
            Assert.AreEqual(1, tour.Availabilities.Where(x => x.TourDuration == 6 && x.StartDate == DateTime.Parse("2020-06-20") && x.AvailabilityCount == 9).Count());
            await importer.ExecuteAsync(1);
            tour = await tourRepository.GetTourAsync(1, "EUR123");

            //Assert
            Assert.AreEqual(1, tour.Availabilities.Where(x => x.TourDuration == 6 && x.StartDate == DateTime.Parse("2020-06-20") && x.AvailabilityCount == 2).Count());
        }

        [Test]
        public async Task Importer_AddNewAvailabilityWhereProviderDoesNotExist_AvailabilityShouldNotBeAdded()
        {
            // Arrange
            TestFactory testFactory = new TestFactory();

            List<Availability> availabilities = new List<Availability>()
                {
                    new Availability()
                    {
                        ProductCode = "EUR12345",
                        DepartureDate = "2020-06-20",
                        Nights = 5,
                        Price = 500.0m,
                        Spaces = 8
                    }
                };

            TourRepository tourRepository = new TourRepository();
            ProviderRepository providerRepository = new ProviderRepository();

            IImporter importer = testFactory.CreateImporter(
                tourRepository,
                providerRepository,
                testFactory.CreateMockApiDownloader(availabilities).Object
                );

            // Act
            var tour = await tourRepository.GetTourAsync(5, "EUR12345");
            Assert.IsNull(tour);
            await importer.ExecuteAsync(5);
            tour = await tourRepository.GetTourAsync(5, "EUR12345");

            //Assert
            Assert.IsNull(tour);
        }
    }
}
