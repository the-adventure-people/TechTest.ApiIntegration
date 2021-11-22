using Newtonsoft.Json;
using System.Collections.Generic;

namespace ApiIntegration.Models.Response
{
    public class AvailabilityListResponse
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty("body")]
        public IList<AvailabilityResponse> AvailabilityList { get; set; }
    }
}
