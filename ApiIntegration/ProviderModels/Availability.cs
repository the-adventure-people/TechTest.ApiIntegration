using System.Runtime.Serialization;

namespace ApiIntegration.ProviderModels
{
    [DataContract]
    public class Availability
    {
        [DataMember(Name = "productCode")]
        public string ProductCode { get; set; }

        [DataMember(Name = "departureDate")]
        public string DepartureDate { get; set; }

        [DataMember(Name = "nights")]
        public int Nights { get; set; }

        [DataMember(Name = "price")]
        public int Price { get; set; }

        [DataMember(Name = "spaces")]
        public int Spaces { get; set; }
    }
}
