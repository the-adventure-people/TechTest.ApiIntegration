using System.Collections.Generic;

namespace ApiIntegration.ProviderModels
{
    public class ApiAvailabilityResponse
    {
        public int StatusCode { get; set; }
        public List<Availability> Body { get; set; }
    }
}
