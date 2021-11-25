using ApiIntegration.Models;
using System;
using System.Globalization;

namespace ApiIntegration.ProviderModels
{
    public class Availability
    {
        public string ProductCode { get; set; }
        public string DepartureDate { get; set; }
        public int Nights { get; set; }
        public decimal Price { get; set; }
        public int Spaces { get; set; }

        public TourAvailability ToTourAvailability()
        {
            var ta = new TourAvailability();
            ta.StartDate = DateTime.ParseExact(DepartureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            ta.TourDuration = Nights;
            ta.SellingPrice = Price;
            ta.AvailabilityCount = Spaces;

            return ta;
        }
    }
}
