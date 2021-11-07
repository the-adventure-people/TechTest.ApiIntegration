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

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                TourAvailability c = (TourAvailability)obj;
                return TourId == c.TourId && StartDate.Equals(c.StartDate) && TourDuration == c.TourDuration && SellingPrice == c.SellingPrice && AvailabilityCount == c.AvailabilityCount;
            }
        }
    }
}
