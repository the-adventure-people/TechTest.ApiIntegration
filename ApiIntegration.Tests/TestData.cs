namespace ApiIntegration.Tests
{
    using ApiIntegration.Models;
    using ApiIntegration.ProviderModels;
    using System;
    using System.Collections.Generic;

    public static class TestData
    {
        public static ApiAvailabilityResponse TestAvailabilityData()
        {
            var testData = new ApiAvailabilityResponse
            {
                StatusCode = 200,
                Body = new List<Availability>()
            };

            testData.Body.Add(new Availability
            {
                ProductCode = "EUR001",
                DepartureDate = "2023-06-20",
                Nights = 5,
                Price = 500.0m,
                Spaces = 8
            });

            testData.Body.Add(new Availability
            {
                ProductCode = "EUR002",
                DepartureDate = "2023-07-10",
                Nights = 5,
                Price = 450.0m,
                Spaces = 4
            });

            return testData;
        }


        public static Tour TestTour()
        {
            return new Tour()
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
            };
        }
    }
}
