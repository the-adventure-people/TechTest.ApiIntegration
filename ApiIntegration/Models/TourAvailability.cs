using ApiIntegration.Models.Response;
using System;

namespace ApiIntegration.Models
{
    public class TourAvailability
    {
        public TourAvailability()
        {

        }

        public TourAvailability(AvailabilityResponse entity) 
        {
            TourId = entity.TourId;
            StartDate = entity.DepartureDate;
            TourDuration = entity.Nights;
            SellingPrice = entity.Price;
            AvailabilityCount = entity.Spaces;
        }

        //obj casting
        public static implicit operator TourAvailability(AvailabilityResponse entity) => new TourAvailability(entity);

        public int TourId { get; set; }
        public DateTime StartDate { get; set; }
        public int TourDuration { get; set; }
        public decimal SellingPrice { get; set; }
        public int AvailabilityCount { get; set; }
    }
}
