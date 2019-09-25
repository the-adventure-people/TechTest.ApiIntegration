using System.Collections.Generic;

namespace ApiIntegration.Models.Entities
{
    public class TourEntity
    {
        public int TourId { get; set; }
        public string TourRef { get; set; }
        public int ProviderId { get; set; }
        public string TourName { get; set; }
        public decimal ReviewScore { get; set; }
        public int ReviewCount { get; set; }
        public bool Active { get; set; }
        public List<TourAvailabilityEntity> Availabilities { get; set; }
    }
}
