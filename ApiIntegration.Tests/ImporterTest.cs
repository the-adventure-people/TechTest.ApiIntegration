using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ApiIntegration.Tests
{
    [TestFixture]
    public class ImporterTest
    {
        private Mock<ITourRepository> mockTourRepository;
        private Mock<IProviderRepository> mockProviderRepository;
        private Mock<IApiDownloader> mockApiDownloader;
        private Mock<ILogger> logger;

        public void Setup()
        {
            mockTourRepository = new Mock<ITourRepository>();
            mockProviderRepository = new Mock<IProviderRepository>();
            mockApiDownloader = new Mock<IApiDownloader>();
            logger = new Mock<ILogger>();

            mockProviderRepository.Setup(x => x.Get(1)).Returns(Task.FromResult(new Provider()
            {
                ProviderId = 1,
                Name = "Awesome Cycling Holidays",
                Commission = 0.15m
            }));
            mockTourRepository.Setup(x => x.Get(default, "EUR123")).Returns(Task.FromResult(new Tour()
            {
                TourId = 1,
                TourRef = "EUR123",
                TourName = "Cycling Danube",
                ProviderId = 1,
                Active = true,
                ReviewCount = 13,
                ReviewScore = 4.3m,
                Availabilities = new List<TourAvailability>()
                {
                    new TourAvailability()
                    {
                        TourId = 1,
                        SellingPrice = 500,
                        StartDate = new DateTime(2020, 6, 20),
                        TourDuration = 6,
                        AvailabilityCount = 9
                    },
                    new TourAvailability()
                    {
                        TourId = 1,
                        SellingPrice = 450,
                        StartDate = new DateTime(2020, 6, 27),
                        TourDuration = 6,
                        AvailabilityCount = 9
                    }
                }
            }));
            mockTourRepository.Setup(x => x.Get(default, "EUR456")).Returns(Task.FromResult(new Tour()
            {
                TourId = 2,
                TourRef = "EUR456",
                TourName = "Cycling Rhine",
                ProviderId = 1,
                Active = true,
                ReviewCount = 55,
                ReviewScore = 4.8m,
                Availabilities = new List<TourAvailability>()
                {
                    new TourAvailability()
                    {
                        TourId = 2,
                        SellingPrice = 720,
                        StartDate = new DateTime(2020, 3, 10),
                        TourDuration = 11,
                        AvailabilityCount = 4
                    },
                    new TourAvailability()
                    {
                        TourId = 2,
                        SellingPrice = 720,
                        StartDate = new DateTime(2020, 3, 20),
                        TourDuration = 11,
                        AvailabilityCount = 5
                    }
                }
            }));
            mockApiDownloader.Setup(x => x.Download()).Returns(Task.FromResult(new ApiAvailabilityResponse
            {
                StatusCode = 200,
                Body = new List<Availability>
                {
                    new Availability
                    {
                        ProductCode = "EUR123",
                        DepartureDate = "2020-06-20",
                        Nights = 5,
                        Price = 500,
                        Spaces = 8
                    },
                    new Availability
                    {
                        ProductCode = "EUR123",
                        DepartureDate = "2020-06-27",
                        Nights = 5,
                        Price = 450,
                        Spaces = 4
                    },
                    new Availability
                    {
                        ProductCode = "EUR123",
                        DepartureDate = "2020-07-04",
                        Nights = 5,
                        Price = 500,
                        Spaces = 6
                    },
                    new Availability
                    {
                        ProductCode = "EUR456",
                        DepartureDate = "2020-03-10",
                        Nights = 10,
                        Price = 800,
                        Spaces = 4
                    },
                    new Availability
                    {
                        ProductCode = "EUR456",
                        DepartureDate = "2020-03-20",
                        Nights = 10,
                        Price = 800,
                        Spaces = 5
                    },
                    new Availability
                    {
                        ProductCode = "EUR789",
                        DepartureDate = "2020-09-20",
                        Nights = 4,
                        Price = 250,
                        Spaces = 9
                    }
                }
            }));
        }

        [Test]
        public async Task Correct_Status_Code_Produces_Correct_Update()
        {
            Setup();
            var sut = new Importer(mockTourRepository.Object, mockProviderRepository.Object, mockApiDownloader.Object,
                logger.Object);
            var actual = new List<Tour>();
            mockTourRepository.Setup(x => x.Update(It.IsAny<Tour>())).Callback<Tour>(x => actual.Add(x));

            await sut.Execute(1);

            var expected = new List<Tour>
            {
                new Tour()
                {
                    TourId = 1,
                    TourRef = "EUR123",
                    TourName = "Cycling Danube",
                    ProviderId = 1,
                    Active = true,
                    ReviewCount = 13,
                    ReviewScore = 4.3m,
                    Availabilities = new List<TourAvailability>()
                    {
                        new TourAvailability()
                        {
                            TourId = 1,
                            SellingPrice = 550,
                            StartDate = new DateTime(2020, 6, 20),
                            TourDuration = 5,
                            AvailabilityCount = 8
                        },
                        new TourAvailability()
                        {
                            TourId = 1,
                            SellingPrice = 495,
                            StartDate = new DateTime(2020, 6, 27),
                            TourDuration = 5,
                            AvailabilityCount = 4
                        },
                        new TourAvailability()
                        {
                            TourId = 1,
                            SellingPrice = 550,
                            StartDate = new DateTime(2020, 7, 04),
                            TourDuration = 5,
                            AvailabilityCount = 6
                        }
                    }
                },
                new Tour()
                {
                    TourId = 2,
                    TourRef = "EUR456",
                    TourName = "Cycling Rhine",
                    ProviderId = 1,
                    Active = true,
                    ReviewCount = 55,
                    ReviewScore = 4.8m,
                    Availabilities = new List<TourAvailability>()
                    {
                        new TourAvailability()
                        {
                            TourId = 2,
                            SellingPrice = 880,
                            StartDate = new DateTime(2020, 3, 10),
                            TourDuration = 10,
                            AvailabilityCount = 4
                        },
                        new TourAvailability()
                        {
                            TourId = 2,
                            SellingPrice = 880,
                            StartDate = new DateTime(2020, 3, 20),
                            TourDuration = 10,
                            AvailabilityCount = 5
                        }
                    }
                }
            };
            CollectionAssert.AreEquivalent(expected,actual);
        }

        [Test]
        public async Task Incorrect_Status_Code_Finishes_Without_Update()
        {
            Setup();
            var sut = new Importer(mockTourRepository.Object, mockProviderRepository.Object, mockApiDownloader.Object,
                logger.Object);

            mockApiDownloader.Setup(x => x.Download()).Returns(Task.FromResult(new ApiAvailabilityResponse
            {
                StatusCode = 404,
                Body = null
            }));

            await sut.Execute(1);

            mockTourRepository.Verify(x => x.Update(It.IsAny<Tour>()), Times.Never);
        }

        [Test]
        public async Task Download_Expception_Finishes_Without_Update()
        {
            Setup();
            var sut = new Importer(mockTourRepository.Object, mockProviderRepository.Object, mockApiDownloader.Object,
                logger.Object);

            mockApiDownloader.Setup(x => x.Download()).Throws(new Exception());

            await sut.Execute(1);

            mockTourRepository.Verify(x => x.Update(It.IsAny<Tour>()), Times.Never);
        }
    }
}
