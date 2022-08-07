namespace ApiIntegration.Models
{
    using System;

    public class PricedTourAvailability
    {
        public int TourId { get; init; }
        public DateTime StartDate { get; init; }
        public int TourDuration { get; init; }
        public decimal SellingPrice { get; init; }
        public int AvailabilityCount { get; init; }
    }
}
