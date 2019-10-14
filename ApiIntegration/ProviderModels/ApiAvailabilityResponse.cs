using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiIntegration.ProviderModels
{
    public class ApiAvailabilityResponse
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("body")]
        public List<Availability> Body { get; set; }
    }
}
