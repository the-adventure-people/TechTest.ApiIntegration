namespace ApiIntegration.Models
{
    using System.Collections.Generic;

    public class Tour
    {
        public int TourID { get; init; }
        public string TourRef { get; init; }
        public int ProviderId { get; init; }
        public string TourName { get; init; }
        public decimal ReviewScore { get; init; }
        public int ReviewCount { get; init; }
        public bool Active { get; init; }
        public List<PricedTourAvailability> Availabilities { get; init; }
    }
}
