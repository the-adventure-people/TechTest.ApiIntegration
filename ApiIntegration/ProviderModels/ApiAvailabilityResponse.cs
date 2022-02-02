namespace ApiIntegration.ProviderModels
{
    using System.Collections.Generic;

    public class ApiAvailabilityResponse
    {
        public int StatusCode { get; set; }
        public IList<Availability> Body { get; set; }
    }
}
