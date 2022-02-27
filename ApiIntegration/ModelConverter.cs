using ApiIntegration.Models;
using ApiIntegration.ProviderModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiIntegration
{
    public class ModelConverter
    {
        public List<TourAvailability> ConvertToTourAvailability(ApiAvailabilityResponse availabilityResponse)
        {
            var tours = new List<TourAvailability>();
            
            foreach(var availability in availabilityResponse.Body)
            {
                tours.Add(new TourAvailability()
                {
                    AvailabilityCount = availability.Spaces,
                    SellingPrice = availability.Price,
                    StartDate = DateTime.Parse(availability.DepartureDate),
                    TourDuration = availability.Nights,
                    TourId = Convert.ToInt32(availability.ProductCode.Substring(availability.ProductCode.Length - 3))
                });
            }

            return tours;
        }
    }
}
