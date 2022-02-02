namespace ApiIntegration.Models
{
    using System.Collections.Generic;

    public class Tour
    {
        public int TourId { get; set; }
        public string TourRef { get; set; }
        public int ProviderId { get; set; }
        public string TourName { get; set; }
        public decimal ReviewScore { get; set; }
        public int ReviewCount { get; set; }
        public bool Active { get; set; }
        public List<TourAvailability> Availabilities { get; set; }
    }
}
