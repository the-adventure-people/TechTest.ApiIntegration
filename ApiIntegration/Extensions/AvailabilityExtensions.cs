using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Extensions
{
    public static class AvailabilityExtensions
    {
        public static TourAvailability ToTourAvailability(this Availability availability, int tourId)
        {
            return new TourAvailability()
            {
                TourId = tourId,
                StartDate = DateTime.Parse(availability.DepartureDate),
                TourDuration = availability.Nights,
                SellingPrice = availability.Price,
                AvailabilityCount = availability.Spaces
            };
        }
    }
}
