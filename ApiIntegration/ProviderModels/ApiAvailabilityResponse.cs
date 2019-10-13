using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ApiIntegration.ProviderModels
{
    [DataContract]
    public class ApiAvailabilityResponse
    {
        [DataMember(Name = "statusCode")]
        public int StatusCode { get; set; }

        [DataMember(Name = "body")]
        public List<Availability> Body { get; set; }
    }
}
