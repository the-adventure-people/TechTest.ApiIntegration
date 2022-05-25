using System.Collections.Generic;

namespace ApiIntegration.Data.Models.External
{
    public class ApiAvailabilityResponse
    {
        public int StatusCode { get; set; }
        public List<Availability> Body { get; set; }
    }
}
