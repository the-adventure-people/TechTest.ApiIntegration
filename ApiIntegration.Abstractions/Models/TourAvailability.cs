namespace ApiIntegration.Models
{
    using System;

    public class TourAvailability
    {
        public int TourID { get; init; }
        public DateTime StartDate { get; init; }
        public int TourDuration { get; init; }
        public decimal Cost { get; init; }
        public int AvailabilityCount { get; init; }
    }
}
