using ApiIntegration.Models.ResponseModels;
using System;
using System.Collections.Generic;

namespace ApiIntegration.Models.Entities
{
    public class TourAvailabilityEntity
    {
        public int TourId { get; set; }
        public DateTime StartDate { get; set; }
        public int TourDuration { get; set; }
        public decimal AdultPrice { get; set; }
        public int AvailabilityCount { get; set; }
        //dont write this to the db
        public string ProductCode { get; set; }

        public static IEnumerable<TourAvailabilityEntity> ToTourAvailabilityEntity(List<Availability> availabity, int tourId)
        {
            //automapper?
            foreach (var item in availabity)
            {
                yield return new TourAvailabilityEntity()
                {
                    ProductCode = item.ProductCode,
                    AdultPrice = item.Price,
                    AvailabilityCount = item.Spaces,
                    StartDate = DateTime.Parse(item.DepartureDate),
                    TourDuration = item.Nights,
                    TourId = tourId
                };
            }
     
        }
    }
}
