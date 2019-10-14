using Newtonsoft.Json;

namespace ApiIntegration.ProviderModels
{
    public class Availability
    {
        [JsonProperty("productCode")]
        public string ProductCode { get; set; }

        [JsonProperty("departureDate")]
        public string DepartureDate { get; set; }

        [JsonProperty("nights")]
        public int Nights { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("spaces")]
        public int Spaces { get; set; }
    }
}
