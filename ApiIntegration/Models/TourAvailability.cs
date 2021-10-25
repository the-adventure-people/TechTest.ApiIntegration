using System;

namespace ApiIntegration.Models
{
    public class TourAvailability
    {
        public int TourId { get; set; }
        public DateTime StartDate { get; set; }
        public int TourDuration { get; set; }
        public decimal SellingPrice { get; set; }
        public int AvailabilityCount { get; set; }
    }
}
