using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiIntegration.Models
{
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

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Tour c = (Tour)obj;
                return TourId == c.TourId && TourRef.Equals(c.TourRef) && ProviderId == c.ProviderId && TourName.Equals(c.TourName) && ReviewScore == c.ReviewScore && ReviewCount == c.ReviewCount
                    && Active == c.Active && Availabilities.SequenceEqual(c.Availabilities);
            }
        }
    }
}
