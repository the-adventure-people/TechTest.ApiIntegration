using System.Collections.Generic;

namespace ApiIntegration.Models.ResponseModels
{

    //Thanks for this :)
    public class ApiAvailabilityResponse
    {

        public int StatusCode { get; set; }
        public List<Availability> Body { get; set; }
    }

    public class Availability
    {
        public string ProductCode { get; set; }
        public string DepartureDate { get; set; }
        public int Nights { get; set; }
        public decimal Price { get; set; }
        public int Spaces { get; set; }
    }
}
